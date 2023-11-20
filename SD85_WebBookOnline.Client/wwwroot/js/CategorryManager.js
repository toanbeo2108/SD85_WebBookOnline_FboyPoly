$(document).ready(function () {

    $('body').on('click', '#btn_themm', function () {
        setData(null);
        $('#pp_modal').modal('show');
    })
    $('body').on('click', '#btn_chitiet', function () {
        let id = $(this).attr('data-id')
        $.get('/detail-category/' + id, function (re) {
            if (re.status) {
                setData(re.data)  
                $('#pp_modal').modal('show');
            }
            else {
                alert(re.status)
            }
        })
    })
    //$('body').on('click', '#btn_xoa', function () {
    //    let id = $(this).attr('data-id')
    //    $.get('/detail-category/' + id, function (re) {
    //        if (re.status) {
    //            setData(re.data)
    //            $('#pp_modal').modal('show');
    //        }
    //        else {
    //            alert(re.status)
    //        }
    //    })
    //})
    $('body').on('click', '#btn_save', function () {
        var send = getData();
        if ($('#btn_ID').val() == null || $('#btn_ID').val() == undefined || $('#btn_ID').val() == '')
        {
            $.post('/add-category', { bk: send }, function (re) {
                if (re.status) {
                    alert(re.message);
                    $('#pp_modal').modal('hide');
                    window.location.reload();
                }
                else {
                    alert(re.message)
                }
            })
        }
        else {
            let id = $('#btn_ID').val()
            $.post('/update-categorys/' + id, { vc: send }, function (re) {
                if (re.status) {
                    alert(re.message);
                    $('#pp_modal').modal('hide');
                    window.location.reload();   
                }
                else {
                    alert(re.message)
                }
            })
                
        }
    })

})
function getData() {
    return {
        CategoryID:  $('#btn_ID').val(),
        Name:        $('#btn_name').val(),
        Description: $('#btn_Description').val(),
        Status:      $('#btn_Status').val(),
    }
}        
function setData(data) {
    if (data != null && data != undefined && data != '') {

        $('#btn_ID').val(data.categoryID);
        $('#btn_name').val(data.name);
        $('#btn_Description').val(data.description);
        $('#btn_Status').val(data.status);

    }
    else {
        $('#btn_ID').val('');
        $('#btn_name').val('');
        $('#btn_Description').val('');
        $('#btn_Status').val('');
    }
}