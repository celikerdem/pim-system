$('.custom-file-input').on('change', function () {
    var fileName = $(this).val().split('\\').slice(-1)[0];
    $('.custom-file-label').html(fileName);
})

$('#btnUploadFile').on('click', function () {
    fileUploadProgressStarted();
    setFileUploadInitializing();
    setTimeout(function () {
        if (!postFileUpload()) {
            resetFileUploadProgress();
            fileUploadProgressFinished();
        };
    }, 1000);
})

function fileUploadProgressStarted() {
    resetFileUploadProgress();
    $('#btnUploadFile').addClass('disabled');
    $('#btnGrpUploadFile').hide();
}

function monitorFileUploadProgress(uploadId) {
    var progress = getUploadProgress(uploadId);
    var intervalPeriod = 10000;
    if (progress.totalCount <= 100) {
        intervalPeriod = 100;
    }
    else if (progress.totalCount <= 1000) {
        intervalPeriod = 500;
    }
    else if (progress.totalCount <= 5000) {
        intervalPeriod = 2000;
    }
    var interval = setInterval(function () {
        if (progress.totalCount == progress.successCount + progress.failCount) {
            clearInterval(interval);
            fileUploadProgressFinished();
        } else {
            progress = getUploadProgress(uploadId);
        }
    }, intervalPeriod);
}

function fileUploadProgressFinished() {
    $('#btnUploadFile').removeClass('disabled');
    $('#btnGrpUploadFile').show();
}

function stripProgressBarStyle() {
    var progressBar = $('.progress-bar');
    progressBar.removeClass('progress-bar-striped');
    progressBar.removeClass('progress-bar-animated');
    progressBar.removeClass('bg-success');
    progressBar.removeClass('bg-danger');
    progressBar.removeClass('bg-warning');
}

function resetFileUploadProgress() {
    var progressBar = $('.progress-bar');
    progressBar.css('width', 0 + '%').html('');
    stripProgressBarStyle();
}

function setFileUploadInitializing() {
    var progressBar = $('.progress-bar');
    stripProgressBarStyle();
    progressBar.addClass('progress-bar-striped');
    progressBar.addClass('progress-bar-animated');
    progressBar.addClass('bg-warning');
    progressBar.css('width', 100 + '%').html('File upload in progress...');
}

function setFileUploadProgress(percentage, inprogress) {
    var progressBar = $('.progress-bar');
    progressBar.css('width', percentage + '%').html(percentage + '%');

    if (inprogress === true) {
        stripProgressBarStyle();
        progressBar.addClass('progress-bar-striped');
        progressBar.addClass('progress-bar-animated');
    }
    else if (percentage < 100) {
        progressBar.css('width', 100 + '%');
        stripProgressBarStyle();
        progressBar.addClass('bg-danger');
    }
    else if (percentage == 100) {
        stripProgressBarStyle();
        progressBar.addClass('bg-success');
    }
}