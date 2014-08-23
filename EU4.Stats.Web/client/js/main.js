function upload(file) {
    var xhr = new XMLHttpRequest();
    xhr.open('post', '/games', true);
    xhr.onload = function(e) {
        if (this.status === 200) {
            window.location.href = this.response;
        }
        else if (this.status === 500) {
            $('.spinner').hide();
            var data = JSON.parse(this.response);
            $('div.error').fadeIn();
            $('#errorText').text("ERROR: " + data.error.message);
        }
        else if (this.status === 503) {
            $('.spinner').hide();
            $('div.error').fadeIn();
            overload(60);
        }
    };
    
    var extension = file.name.substr(file.name.lastIndexOf('.'));

    // If the user uploads a plain .eu4, compress the file before sending it.
    // This saves time and bandwidth.
    if (extension === ".eu4") {
        var reader = new FileReader();
        reader.onload = function(buf) {
            var zip = new JSZip();
            zip.file("dummy.eu4", this.result);
            file = zip.generate({type: 'blob', compression: 'DEFLATE'});
            xhr.setRequestHeader("X-FILE-EXTENSION", ".zip");
            xhr.send(file);
        };

        reader.readAsArrayBuffer(file);
    }
    else {
        xhr.setRequestHeader("X-FILE-EXTENSION", extension);
        xhr.send(file);
    }
}

function overload(seconds) {
    if (seconds !== 0) {
        setTimeout(overload, 1000, seconds - 1);
        $('#errorText').text(overloadText(seconds));
    }
    else {
        $('#errorText').text('Retrying...');
        uploadSelectedFile();
    }
}

function overloadText(seconds) {
   return "Sorry, it looks like this service is popular right now, and we can't " +
     "handle the load. We'll resend the savegame in " + seconds + " seconds";
}

function uploadSelectedFile() {
    $('.spinner').show();
    var files = $('#savefile').get(0).files;
    if (files && files[0]) {
        upload(files[0]);
    }
}

$(function() {
    $('#submit').click(uploadSelectedFile);
});
