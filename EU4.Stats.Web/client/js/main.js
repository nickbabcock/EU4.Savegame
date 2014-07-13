function upload(file) {
    var xhr = new XMLHttpRequest();
    xhr.open('post', '/games', true);
    xhr.onload = function(e) {
        if (this.status === 200) {
            window.location.href = this.response;
        }
        else if (this.status === 500) {
            var data = JSON.parse(this.response);
            $('div.error').fadeIn();
            $('#errorText').text("ERROR: " + data.error.message);
        }
    };
    
    var extension = file.name.substr(file.name.lastIndexOf('.'));
    xhr.setRequestHeader("X-FILE-EXTENSION", extension);
    xhr.send(file);
}

$(function() {
    $('#submit').click(function() {
        var file = $('#savefile').get(0).files[0];
        upload(file);
    });
});
