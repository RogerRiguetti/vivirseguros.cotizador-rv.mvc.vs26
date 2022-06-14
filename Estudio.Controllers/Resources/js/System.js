//url: ((method === undefined) ? $('#_url').val() : method),
function sendValues(fields, doSuccess, doError, method) {
    
    $(document.body).css({ 'cursor': 'wait' });
    $.ajax({
        url: method || $('#url').val(),
        type: "POST",
        data: fields || {},
        datatype: "json",
        success: function (result) {
            if (result.IsOk) {
                if (doSuccess === undefined)
                    success();
                else
                    doSuccess(result);

                $(document.body).css({ 'cursor': 'default' });
            }
            else {
                if (doError === undefined)
                    error();
                else
                    doError(result);

                $(document.body).css({ 'cursor': 'default' });
            }
        },
        error: function (result) {
            if (doError === undefined)
                error();
            else
                doError(result);
            $(document.body).css({ 'cursor': 'default' });
        }
    });
}

function sendValuesMetodo(fields, Metodo) {

    //var jstring = '{ ';
    //for (var i = 0; i < fields.length; i++) {
    //    jstring = jstring + fields[i]+  ',';
    //}
    //jstring = jstring.substring(0, jstring.length - 1);
    //jstring = jstring + '}';

    //var dataObject = JSON.stringify(jstring);

    //console.log(fields);

    $.ajax({
        url: Metodo,
        type: "POST",
        data: fields,
        datatype: "json",
        success: function (result) {
            console.dir(result);
            if (result.IsOk) {
                //alert("exito");
                doSuccess();
            }
            else {
                alert(result.Message);
                doSome();
            }
        },
        error: function (result) {
            alert(result.Message);
            doSome();
        }
    });
}


function getData(url, params, callback) {
    $.ajax({
        url: url,
        type: "POST",
        data: params,
        datatype: "json",
        success: function (result) {
            callback(result);
        },
        error: function (result) {
            callback(result);
        }
    });
}