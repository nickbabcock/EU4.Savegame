function statusUpdate(text) {
    $('div.submit-box p').text(text);
}

function upload(file) {
    var xhr = new XMLHttpRequest();
    xhr.open('post', '/games', true);
    xhr.onload = function(e) {
        if (this.status === 200) {
            window.location.href = this.response;
        }
        else if (this.status === 500) {
            statusUpdate('');
            $('progress').hide();
            var data = JSON.parse(this.response);
            $('div.error').fadeIn();
            $('#errorText').text("ERROR: " + data.error.message);
        }
        else if (this.status === 503) {
            statusUpdate('');
            $('progress').hide();
            $('div.error').fadeIn();
            overload(60);
        }
        else {
            statusUpdate('');
            $('progress').hide();
            $('div.error').fadeIn();
            $('#errorText').text("ERROR: We're not sure what went wrong, but " +
                " something did! Inform someone!");
        }
    };

    var progressBar = document.querySelector('progress');
    xhr.upload.onprogress = function(e) {
        if (e.lengthComputable) {
            // Magic number 67 chosen to represent that uploading
            // takes about 2/3 of the time. Of course this depends.
            progressBar.value = (e.loaded / e.total) * 67;
            progressBar.textContent = progressBar.value;
        }
    };

    xhr.upload.onload = function(e) {
        statusUpdate('Processing file');
    };
    
    var extension = file.name.substr(file.name.lastIndexOf('.'));

    // If the user uploads a plain .eu4, compress the file before sending it.
    // This saves time and bandwidth.
    if (extension === ".eu4") {
        var reader = new FileReader();
        reader.onload = function(buf) {
            // In newer version of EU4, players have the option of compressing
            // the savegame. While the file extension is the same, the first
            // couple bytes will let us know if we are looking at a zip file.
            // If we are looking at a zip file, then we don't have to do any
            // compression and we send the file along
            var zipMagic = new Int32Array(this.result.slice(0, 4));
            if (zipMagic[0] !== 0x4034B50) {
                statusUpdate('Compressing file');
                var zip = new JSZip();
                zip.file("dummy.eu4", this.result);
                file = zip.generate({type: 'blob', compression: 'DEFLATE'});
            }

            xhr.setRequestHeader("X-FILE-EXTENSION", ".zip");
            xhr.send(file);
            statusUpdate('Uploading file');
        };

        statusUpdate('Reading file');
        reader.readAsArrayBuffer(file);
    }
    else {
        xhr.setRequestHeader("X-FILE-EXTENSION", extension);
        statusUpdate('Uploading file');
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
    $('progress').fadeIn();
    var files = $('#savefile').get(0).files;
    if (files && files[0]) {
        upload(files[0]);
    }
}

$(function() {
    $('#submit').click(uploadSelectedFile);
});
