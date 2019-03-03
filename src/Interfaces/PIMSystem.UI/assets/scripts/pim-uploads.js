var PIM_UPLOADS_ENDPOINT = '/uploads';

$.support.cors = true;

function postFileUpload() {
    var file = $('#inputFileUpload').get(0).files[0];

    if (!file) {
        return false;
    }

    var fileData = new FormData();
    fileData.set('file', file, file.name);

    $.ajax({
        url: PIM_API_BASE_URL + PIM_UPLOADS_ENDPOINT + '/csv',
        type: 'POST',
        data: fileData,
        crossDomain: true,
        cache: false,
        contentType: false,
        processData: false,
        async: false,
        success: function (response) {
            popToast('Uploads', 'File uploaded successfuly. Please check the progress.', 'success');
            monitorFileUploadProgress(response.id);
        },
        error: function (err) {
            console.log(err);
            setFileUploadProgress(0, false);
            fileUploadProgressFinished();
            popToast('Uploads', 'File upload failed!', 'danger');
        }
    });

    return true;
}

function getUploadProgress(id) {
    let progressResponse;
    $.ajax({
        url: PIM_API_BASE_URL + PIM_UPLOADS_ENDPOINT + '/' + id + '/progress',
        type: 'GET',
        crossDomain: true,
        cache: false,
        contentType: false,
        processData: false,
        async: false,
        success: function (response) {
            var progressPercentage = parseInt(response.successCount * 100 / response.totalCount);
            var inProgress = response.successCount + response.failCount == response.totalCount;
            setFileUploadProgress(progressPercentage, !inProgress);
            progressResponse = response;
        },
        error: function (err) {
        }
    });
    return progressResponse;
}