$(document).ready(function () {
    ///



    $('body').on('click', '#themmoi', function () {
        setData('');
        $('#exampleModal').modal('show');

    })

    $('body').on('click', '#xemchitiet', function (re) {
        let id = $(this).attr('data-id');

        $.get('/detail-inp/' + id, function (re) {
            if (re.status) {
                setData(re.data)
                $('#exampleModal').modal('show');

            }
            else {
                alert(re.message)
            }
        })
    })

    $('body').on('click', '#btn-them', function () {
        var send = getData();

        if ($('#btn_inpID').val() == null || $('#btn_inpID').val() == '' || $('#btn_inpID').val() == undefined) {
            $.post('/themm-inputslip', { ip: send }, function (re) {
                if (re.status) {

                    alert(re.message);
                    $('#exampleModal').modal('hide');

                    window.location.reload();
                }
                else {
                    alert(re.message);

                }
            })
        }
        else {
            let id = $('#btn_inpID').val();
            $.post('/update-inp/' + id, { ip: send }, function (re) {

                if (re.status) {

                    alert(re.message);
                    $('#exampleModal').modal('hide');

                    window.location.reload();
                }
                else {
                    alert(re.message);

                }
            })
        }

    });
})

function setData(data) {

    if (data != null && data != undefined && data != "") {

        $('#btn_inpID').val(data.inputSlipID);
        $('#cb_bookname').val(data.idSachNhap);
        $('#cb_nv').val(data.idNhanVienNhap);
        $('#btn_soluong').val(data.soLuong);
        $('#btn_gianhap').val(data.giaNhap);
        $('#btn_ban').val(data.giaBan);
        $('#btn_ngaynhap').val(data.NgayNhap);


    } else {
        $('#btn_inpID').val('');
        $('#cb_bookname').val('');
        $('#cb_nv').val('');
        $('#btn_soluong').val('');
        $('#btn_gianhap').val('');
        $('#btn_ban').val('');
        $('#btn_ngaynhap').val('');

    }
}

function getData() {
    return {
        InputSlipID: $('#btn_inpID').val(),
        IdSachNhap: $('#cb_bookname').val(),
        IdNhanVienNhap: $('#cb_nv').val(),
        SoLuong: $('#btn_soluong').val(),
        GiaNhap: $('#btn_gianhap').val(),
        GiaBan: $('#btn_ban').val(),
        NgayNhap: $('#btn_ngaynhap').val()
    }
}
