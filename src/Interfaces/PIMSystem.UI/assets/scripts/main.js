$('.toast').toast({
    animation: true,
    autohide: false,
    delay: 5000
});

function getFormattedDate() {
    var date = new Date();
    var str = date.getFullYear() + "-" + (date.getMonth() + 1) + "-" + date.getDate() + " " + date.getHours() + ":" + date.getMinutes() + ":" + date.getSeconds();

    return str;
}

function popToast(title, message, type) {
    var successToastHtml = '<div class="toast fade show" role="alert" aria-live="assertive" aria-atomic="true"><div class="toast-header **type**"><strong class="mr-auto text-white">**title**</strong><span style="min-width:20px;"></span><small class="text-light">**time**</small><button type="button" onclick="$(this).parent().parent().removeClass(\'show\').addClass(\'hide\')" class="ml-2 mb-1 close" data-dismiss="toast" aria-label="Close"><span aria-hidden="true">&times;</span></button></div><div class="toast-body">**message**</div></div>';
    successToastHtml = successToastHtml.replace('**title**', title);
    successToastHtml = successToastHtml.replace('**message**', message);
    successToastHtml = successToastHtml.replace('**time**', getFormattedDate());

    if (type === 'success')
        successToastHtml = successToastHtml.replace('**type**', 'bg-success');
    else if (type === 'danger')
        successToastHtml = successToastHtml.replace('**type**', 'bg-danger');
    else if (type === 'warning')
        successToastHtml = successToastHtml.replace('**type**', 'bg-warning');
    else
        successToastHtml = successToastHtml.replace('**type**', '');

    $('#toastsArea').append(successToastHtml);
}