var PIM_UPLOADS_ENDPOINT = '/uploads';

$.support.cors = true;

function postFileUpload() {
    var file = $('#inputFileUpload').get(0).files[0];

    if (!file) {
        return false;
    }

    var fileData = new FormData();
    fileData.append('uploadfile', file);

    $.ajax({
        url: PIM_API_BASE_URL + PIM_UPLOADS_ENDPOINT,
        type: 'POST',
        data: fileData,
        crossDomain: true,
        xhrFields: { withCredentials: true },
        cache: false,
        contentType: false,
        processData: false,
        success: function (response) {
            popToast('Uploads', 'File uploaded successfuly.', 'success');
        },
        error: function (err) {
            console.log(err);
            popToast('Uploads', 'File upload failed!', 'danger');
        }
    });
}