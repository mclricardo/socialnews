var socialHubClient;
var userInfo;

var Message = function (id, text, author, createdOn, replies, likes) {
    var self = this;

    self.id = ko.observable(id);
    self.text = ko.observable(text);
    self.author = ko.observable(author);
    self.createdOn = ko.observable(createdOn);
    self.likes = ko.observableArray(likes);
    self.newComment = ko.observable('');
    self.itemToAdd = ko.observable(""),
    self.showCommentWatermark = ko.observable(true),
    self.isTypingComment = false,

    self.likedByThisUser = ko.computed(function () {
        var liked = false;
        $(self.likes()).each(function (array, user) {
            if (user.id == userInfo.Id) {
                liked = true;
            }
        });
        return liked;
    });

    self.addLike = function () {
        socialHubClient.sendLikeToServer(self.id());
    } .bind(self);

    self.unlike = function () {
        socialHubClient.sendUnlikeToServer(self.id());
    } .bind(self);

    self.addNewComment = function (id, comment, user, createdOn) {
        self.messages.push(new Message(id, comment, user, createdOn, [], []));
    } .bind(self);

    self.likeSummary = ko.computed(function () {
        var summary = '';
        var sortedLikes = self.likes.sort(function (a, b) {
            var expA = (a.Id == userInfo.Id ? -1 : 1);
            var expB = (b.Id == userInfo.Id ? -1 : 1);
            return expA < expB ? -1 : 1;
        })

        $(sortedLikes).each(function (index, author) {
            if (summary.length > 0) {
                if (index == likes.length - 1) {
                    summary += ' and ';
                }
                else {
                    summary += ', ';
                }
            }

            if (author.name == userInfo.Name) {
                summary += 'You';
            }
            else {
                summary += author.name;
            }
        });
        if (self.likes().length > 0) {
            summary += ' liked this';
        }
        return summary;
    });

    self.messages = ko.observableArray([]);
    $(replies).each(function (index, reply) {
        var lk = [];
        $(reply.Likes).each(function (array, like) {
            lk.push({ id: like.Id, name: like.Name });
        });

        self.messages.push(
            new Message(reply.Id, reply.Text, reply.Author, reply.CreatedOn, [], lk)
        );
    });

    self.commentKeypress = function (msg, event) {
        if (event.keyCode) {
            if (event.keyCode === 13) {
                if (self.newComment().trim().length > 0) {
                    socialHubClient.sendCommentToServer(self.id(), self.newComment());
                    self.newComment('');
                }
                return false;
            }
        }
        return true;
    }

    self.commentClick = function (msg, event) {
        self.showCommentWatermark(false);
    }

    self.commentFocus = function (msg, event) {
        self.isTypingComment = true;
        self.showCommentWatermark(false);
    }

    self.commentFocusout = function (msg, event) {
        if (self.newComment().trim().length == 0) {
            self.isTypingComment = false;
            self.showCommentWatermark(true);
        }
    }

    self.commentMouseEnter = function (msg, event) {
        self.showCommentWatermark(false);
    }

    self.commentMouseLeave = function (msg, event) {
        if (!self.isTypingComment && self.newComment().trim().length == 0) {
            self.showCommentWatermark(true);
        }
    }

    self.timeEllapsed = ko.computed(function () {
        var ellapsed = '';

        var timeStampDiff = (new Date()).getTime() - parseInt(self.createdOn().substring(6, 19));
        var s = parseInt(timeStampDiff / 1000);
        var m = parseInt(s / 60);
        var h = parseInt(m / 60);
        var d = parseInt(h / 24);

        if (d > 1) {
            ellapsed = d + ' days ago';
        }
        else if (d == 1) {
            ellapsed = d + ' day ago';
        }
        else if (h > 1) {
            ellapsed = h + ' hours ago';
        }
        else if (h == 1) {
            ellapsed = h + ' hour ago';
        }
        else if (m > 1) {
            ellapsed = m + ' minutes ago';
        }
        else if (m == 1) {
            ellapsed = m + ' minute ago';
        }
        else if (s > 10) {
            ellapsed = s + ' seconds ago';
        }
        else {
            ellapsed = 'just posted';
        }

        return ellapsed;
    });
}

var viewModel = function (model) {
    var self = this;
    self.messages = ko.observableArray([]);
    self.isSignalREnabled = ko.observable(model.isSignalREnabled);
    self.newComment = ko.observable('');
    self.showCommentWatermark = ko.observable(true),
    self.isTypingComment = false,

    $(model.Messages).each(function (index, message) {
        var likes = [];

        $(message.Likes).each(function (array, like) {
            likes.push({ id: like.Id, name: like.Name });
        });

        self.messages.push(
            new Message(message.Id, message.Text, message.Author, message.CreatedOn, message.Messages, likes)
        );
    });

    self.findMessageAndAct = function (messageId, parent, action) {
        $(parent.messages()).each(function (index, message) {
            if (message.id() == messageId) {
                action(message);
                return false;
            }

            $(message.messages()).each(function (index, message) {
                if (message.id() == messageId) {
                    action(message);
                    return false;
                }
            });
        });
    }

    self.addNewComment = function (id, comment, user, createdOn) {
        self.messages.unshift(new Message(id, comment, user, createdOn, [], []));
    } .bind(self);

    self.commentKeypress = function (msg, event) {
        if (event.keyCode) {
            if (event.keyCode === 13) {
                if (self.newComment().trim().length > 0) {
                    socialHubClient.sendCommentToServer(null, self.newComment());
                    self.newComment('');
                }
                return false;
            }
        }
        return true;
    }

    self.commentClick = function (msg, event) {
        self.showCommentWatermark(false);
    }

    self.commentFocus = function (msg, event) {
        self.isTypingComment = true;
        self.showCommentWatermark(false);
    }

    self.commentFocusout = function (msg, event) {
        if (self.newComment().trim().length == 0) {
            self.isTypingComment = false;
            self.showCommentWatermark(true);
        }
    }

    self.commentMouseEnter = function (msg, event) {
        self.showCommentWatermark(false);
    }

    self.commentMouseLeave = function (msg, event) {
        if (!self.isTypingComment && self.newComment().trim().length == 0) {
            self.showCommentWatermark(true);
        }
    }
};

$(function () {
    window.isSignalREnabled = false;
    requestWallMessages();
});

function requestWallMessages() {
    $.ajax({
        url: "/Home/GetWallMessages",
        dataType: 'json',
        success: function (data) {
            setTimeout(function () {
                data.isSignalREnabled = window.isSignalREnabled;
                window.wallViewModel = new viewModel(data);
                $('#loading-wall-messages').css('display', 'none');
                ko.applyBindings(window.wallViewModel);
                $('.wall-messages').css('display', '');

                setTimeout(function () {
                    setupHubClient();
                }, 100);
            }, 50);
        },
        error: function (jqXHR, textStatus, errorThrown) {
            $('.ajax-error').css('display', '');
            $('#loading-wall-messages').css('display', 'none');
        }
    });
}

function setupHubClient() {
    socialHubClient = $.connection.socialHub;

    // Start the connection
    $.connection.hub.start(function () {
        socialHubClient.join(userInfo.Name);
    }).done(function () {
        window.isSignalREnabled = true;
        if (window.wallViewModel) {
            window.wallViewModel.isSignalREnabled(true);
        }
    }).fail(function () {
        //alert('connection failed');
    });

    socialHubClient.updateLike = function (messageId, personWhoLiked) {
        window.wallViewModel.findMessageAndAct(messageId, wallViewModel, function (message) {
            message.likes.push({
                id: personWhoLiked.Id,
                name: personWhoLiked.Name
            });
        });
    };

    socialHubClient.updateUnlike = function (messageId, personWhoUnliked) {
        window.wallViewModel.findMessageAndAct(messageId, wallViewModel, function (message) {
            for (var i = message.likes().length - 1; i >= 0; i--) {
                if (message.likes()[i].id == personWhoUnliked.Id) {
                    message.likes.splice(i, 1);
                    break;
                }
            }
        });
    };

    socialHubClient.addComment = function (parentMessageId, newMessageId, comment, author) {
        if (parentMessageId) {
            window.wallViewModel.findMessageAndAct(parentMessageId, wallViewModel, function (parentMessage) {
                parentMessage.addNewComment(
                    newMessageId,
                    comment,
                    { Id: author.Id, Name: author.Name, SmallPicturePath: author.SmallPicturePath, MediumPicturePath: author.MediumPicturePath },
                    '/Date(' + (new Date()).getTime() + ')/'
                );
            });
        }
        else {
            window.wallViewModel.addNewComment(
                newMessageId,
                comment,
                { Id: author.Id, Name: author.Name, SmallPicturePath: author.SmallPicturePath, MediumPicturePath: author.MediumPicturePath },
                '/Date(' + (new Date()).getTime() + ')/'
            );
        }
    };
}

function setTimeEllapsedField(message) {
    message.timeEllapsed = ko.computed(function () {
        var ellapsed = '';

        var timeStampDiff = (new Date()).getTime() - parseInt(message.CreatedOn.substring(6, 19));
        var s = parseInt(timeStampDiff / 1000);
        var m = parseInt(s / 60);
        var h = parseInt(m / 60);
        var d = parseInt(h / 24);

        if (d > 1) {
            ellapsed = d + ' days ago';
        }
        else if (d == 1) {
            ellapsed = d + ' day ago';
        }
        else if (h > 1) {
            ellapsed = h + ' hours ago';
        }
        else if (h == 1) {
            ellapsed = h + ' hour ago';
        }
        else if (m > 1) {
            ellapsed = m + ' minutes ago';
        }
        else if (m == 1) {
            ellapsed = m + ' minute ago';
        }
        else if (s > 10) {
            ellapsed = s + ' seconds ago';
        }
        else {
            ellapsed = 'just posted';
        }

        return ellapsed;
    });

    $(message.Messages).each(function (array, answer, index) {
        setTimeEllapsedField(answer);
    });
}

function setLikeSummaryField(message) {
    message.likes = ko.observableArray([
        { Id: 1, Name: "Bungle" },
        { Id: 2, Name: "George" },
        { Id: 3, Name: "Zippy" }
    ]);

    message.likeSummary = ko.computed(function () {
        var summary = '';
        $(message.likes).each(function (array, author, index) {
            if (summary.length > 0) {
                if (index == array.length - 1) {
                    summary += 'and ';
                }
                else {
                    summary += ', ';
                }
            }
            summary += author.Name;
        });
        if (message.Likes.length > 0) {
            summary += ' liked this';
        }
        return summary;
    });

    $(message.Messages).each(function (array, answer, index) {
        setLikeSummaryField(answer);
    });
}

function like(message) {
    socialHubClient.sendLikeToServer(message.Id);

    findMessageAndAct(message.Id, wallViewModel, function (message) {
        message.likes.push({
            Id: userInfo.Id,
            Name: userInfo.Name
        });
    });
}

function unlike(message) {
    socialHubClient.sendUnlikeToServer(message.Id);

    findMessageAndAct(message.Id, wallViewModel, function (message) {
        for (var i = message.likes.length; i >= 0; i--) {
            if (message.likes[i].id == userInfo.Id) {
                message.likes.splice(i, 1);
            }
        }
    });
}
