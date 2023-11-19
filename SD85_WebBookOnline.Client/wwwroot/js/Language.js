$(document).ready(function () {

    $('body').on('click', '#btn_add', function () {
        setdata(null);
        $('#pp_modal').modal('show');

    })
    $('body').on('click', '#btn_chitiet', function () {
        let id = $(this).attr('data-id')
        $.get('/detail-Language/' + id, function (re) {
            if (re.status) {
                setdata(re.data)
                $('#pp_modal').modal('show');
            }
            else {
                alert(re.message)
            }
        })
    })
    $('body').on('click', '#btn_save', function () {

        let id = $('#btn_ID').val();
        if ($('#btn_ID').val() == null || $('#btn_ID').val() == undefined || $('#btn_ID').val() == '')
        {
            $.post('/add-Language', { bk: getdata() }, function (re) {

                if (re.status) {
                    alert(re.message)
                    $('#pp_modal').modal('hide');
                    window.location.reload();
                }
                else {
                    alert(re.message);
                }
            })
        }
        else
        {
            $.post('/update-Languge/'+id, { vc: getdata() }, function (re) {

                if (re.status) {
                    alert(re.message)
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
function setdata(data) {
    if (data == null || data==undefined || data=='') {
        $('#btn_ID').val('');
        $('#btn_name').val('');
        $('#btn_Description').val('');
        $('#btn_Status').val('');
    }
    else {
        $('#btn_ID').val(data.langugeID);
        $('#btn_name').val(data.name);
        $('#btn_Description').val(data.description);
        $('#btn_Status').val(data.status);
    }
}
function getdata() {
    return {
        LangugeID  :$('#btn_ID').val(),
       Name : $('#btn_name').val(),
      Description: $('#btn_Description').val(),
     Status: $('#btn_Status').val(),
    }
}