var PIM_CATEGORIES_ENDPOINT = '/categories';

$.support.cors = true;

function getCategoriesData() {
    var data = { "draw": 1, "columns": [{ "data": 0, "name": "", "searchable": true, "orderable": true, "search": { "value": "", "regex": false } }, { "data": 1, "name": "", "searchable": true, "orderable": true, "search": { "value": "", "regex": false } }, { "data": 2, "name": "", "searchable": true, "orderable": true, "search": { "value": "", "regex": false } }, { "data": 3, "name": "", "searchable": true, "orderable": true, "search": { "value": "", "regex": false } }, { "data": 4, "name": "", "searchable": true, "orderable": true, "search": { "value": "", "regex": false } }, { "data": 5, "name": "", "searchable": true, "orderable": true, "search": { "value": "", "regex": false } }, { "data": 6, "name": "", "searchable": true, "orderable": true, "search": { "value": "", "regex": false } }, { "data": 7, "name": "", "searchable": true, "orderable": true, "search": { "value": "", "regex": false } }, { "data": 8, "name": "", "searchable": true, "orderable": true, "search": { "value": "", "regex": false } }], "order": [{ "column": 0, "dir": "asc" }], "start": 0, "length": 10, "search": { "value": "", "regex": false } }
    console.log(JSON.stringify(data));
    $.ajax({
        url: PIM_API_BASE_URL + PIM_CATEGORIES_ENDPOINT + '/filter',
        datatype: 'json',
        contentType: 'application/json',
        type: 'POST',
        processData: false,
        data: JSON.stringify(data),
        success: function (response) {
            console.log(response);
        }
    });
}
