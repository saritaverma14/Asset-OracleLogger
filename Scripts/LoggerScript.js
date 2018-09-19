function GetProject() {
    var Program = $("#ddlProgram").val();
    var url = "http://" + window.location.host + "/OracleLogger/Project?Program=" + Program;
    $.get(url, function (data) {
        var ddlProject = $("#ddlProject");
        ddlProject.empty();

        ddlProject.append($('<option/>', {
            value: 0,
            text: "Select Project"
        }));
        $.each(data, function (index, itemData) {
            ddlProject.append($('<option/>', {
                value: index.Value,
                text: itemData.Text
            }));
        });
        $("#ddlProject").prop("disabled", false);
        $.ajax({
            type: "POST",
            url: "/OracleLogger/LoggerUI",
            data: '{PROGRAM: "' + Program + '" }',
            contentType: "application/json; charset=utf-8",
            dataType: "html",
            success: function (response) {
                $("#progressBar").attr('value', $("#progressBar").prop('value') + 20);
                $('#divMaskDashboard').html(response);

                //$('#dialog').dialog('open');
            },
            failure: function (response) {
                alert(response.responseText);
            },
            error: function (response) {
                alert(response.responseText);
            }
        });

    });
}

function GetRelease() {
    var Program = $("#ddlProgram").val();
    var Project = $("#ddlProject").val();
    var url = "http://" + window.location.host + "/OracleLogger/Release?Project=" + Project;
    $.get(url, function (data) {
        var ddlRelease = $("#ddlRelease");
        ddlRelease.empty();
        ddlRelease.append($('<option/>', {
            value: 0,
            text: "Select Release"
        }));
        $.each(data, function (index, itemData) {
            ddlRelease.append($('<option/>', {
                value: index.Value,
                text: itemData.Text
            }));
        });
        $("#ddlRelease").prop("disabled", false);
        $.ajax({
            type: "POST",
            url: "/OracleLogger/LoggerUI",
            data: '{PROJECT: "' + Project + '" ,PROGRAM:"' + Program + '"}',
            contentType: "application/json; charset=utf-8",
            dataType: "html",
            success: function (response) {
                $("#progressBar").attr('value', $("#progressBar").prop('value') + 20);
                $('#divMaskDashboard').html(response);

                //$('#dialog').dialog('open');
            },
            failure: function (response) {
                alert(response.responseText);
            },
            error: function (response) {
                alert(response.responseText);
            }
        });

    });
}

function GetApiOperation() {
    var Program = $("#ddlProgram").val();
    var Project = $("#ddlProject").val();
    var Release = $("#ddlRelease").val();
    var url = "http://" + window.location.host + "/OracleLogger/ApiOperation?Release=" + Release;
    $.get(url, function (data) {
        var ddlApiOperation = $("#ddlApiOperation");
        ddlApiOperation.empty();
        ddlApiOperation.append($('<option/>', {
            value: 0,
            text: "Select API Operation"
        }));
        $.each(data, function (index, itemData) {
            ddlApiOperation.append($('<option/>', {
                value: index.Value,
                text: itemData.Text
            }));
        });
        $("#ddlApiOperation").prop("disabled", false);
        $.ajax({
            type: "POST",
            url: "/OracleLogger/LoggerUI",
            data: '{PROJECT: "' + Project + '" ,PROGRAM:"' + Program + '",RELEASE: "' + Release + '" }',
            //data: '{Release: "' + Release + '" }',
            contentType: "application/json; charset=utf-8",
            dataType: "html",
            success: function (response) {
                $("#progressBar").attr('value', $("#progressBar").prop('value') + 20);
                $('#divMaskDashboard').html(response);

                //$('#dialog').dialog('open');
            },
            failure: function (response) {
                alert(response.responseText);
            },
            error: function (response) {
                alert(response.responseText);
            }
        });
    });
}

function GetSprint() {
    var Program = $("#ddlProgram").val();
    var Project = $("#ddlProject").val();
    var Release = $("#ddlRelease").val();
    var ApiOperation = $("#ddlApiOperation").val();
    var url = "http://" + window.location.host + "/OracleLogger/Sprint?ApiOperation=" + ApiOperation;
    $.get(url, function (data) {
        var ddlSprint = $("#ddlSprint");
        ddlSprint.empty();
        ddlSprint.append($('<option/>', {
            value: 0,
            text: "Select Sprint"
        }));
        $.each(data, function (index, itemData) {
            ddlSprint.append($('<option/>', {
                value: index.Value,
                text: itemData.Text
            }));
        });
        $("#ddlSprint").prop("disabled", false);
        $.ajax({
            type: "POST",
            url: "/OracleLogger/LoggerUI",
            data: '{PROJECT: "' + Project + '" ,PROGRAM:"' + Program + '", RELEASE: "' + Release + '" ,APIOPERATION: "' + ApiOperation + '"}',
            // data: '{ApiOperation: "' + ApiOperation + '" }',
            contentType: "application/json; charset=utf-8",
            dataType: "html",
            success: function (response) {
                $("#progressBar").attr('value', $("#progressBar").prop('value') + 20);
                $('#divMaskDashboard').html(response);

                //$('#dialog').dialog('open');
            },
            failure: function (response) {
                alert(response.responseText);
            },
            error: function (response) {
                alert(response.responseText);
            }
        });
        //$("#progressBar").attr('value', $("#progressBar").prop('value') + 30);
        //$("#ddlSprint").prop("disabled", false);
        //$("#divMaskDashboard").show();
    });
}

function GetData() {
    debugger
    var Program = $("#ddlProgram").val();
    var Project = $("#ddlProject").val();
    var Release = $("#ddlRelease").val();
    var ApiOperation = $("#ddlApiOperation").val();
    var Sprint = $("#ddlSprint").val();
    $.ajax({
        type: "POST",
        url: "/OracleLogger/LoggerUI",
        data: '{PROJECT: "' + Project + '" ,PROGRAM:"' + Program + '", RELEASE: "' + Release + '" ,APIOPERATION: "' + ApiOperation + '" , SPRINT:"' + Sprint + '" }',
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        success: function (response) {
            $('#divMaskDashboard').html(response);
        },
        failure: function (response) {
            alert(response.responseText);
        },
        error: function (response) {
            alert(response.responseText);
        }
    });
}
//$(document).ready(function () {
//    alert('hello');
//    $("#Copyrows").click(function (event) {
//        alert('hello');
//        event.preventDefault();
//        var check = false;
//        var val;
//        var ArrayOfIds = [];
//        var i = 0;
//        var ListOfIds;
//        if ($(':checkbox:checked').length <= 0) {
//            event.preventDefault();
//            alert('please select row that you want to copy..');
//            ListOfIds = "";
//            return false;
//        }
//        else {
//            $(':checkbox:checked').each(function (i) {
//                val = $(this).closest('tr').children('td:first').text();
//                ArrayOfIds[i] = val;
//                i = i + 1;
//            });
//            ListOfIds = ArrayOfIds.toString();
//            var trimStr = ListOfIds.replace(/ /g, '');

//            PopUp1 = window.open('../OracleLogger/_CopyDetails/?ArrayIds=' + trimStr + '', null, 'titlebar=0, width=440,height =350,left=500,top=175,scrollbars=no').split('&');
//            PopUp1.focus();
//            event.preventDefault();
//            return false;
//        }
//    });
//});

function checkAll(e) {
    var checkboxes = document.getElementsByName('check');
    if (e.checked) {
        for (var i = 0; i < checkboxes.length; i++) {
            checkboxes[i].checked = true;
        }
    } else {
        for (var i = 0; i < checkboxes.length; i++) {
            checkboxes[i].checked = false;
        }
    }
}
function CallChangefunc(Count) {
    debugger
    var Count = $("#ddlCount").val();
    $.ajax({
        type: "POST",
        url: "/OracleLogger/LoggerUI",
        data: '{ ItemCount:"' + Count + '" }',
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        success: function (response) {
            $('#divMaskDashboard').html(response);
            $("#ddlCount").val(Count);

        },

        failure: function (response) {
            alert(response.responseText);
        },
        error: function (response) {
            alert(response.responseText);
        }
    });
}
