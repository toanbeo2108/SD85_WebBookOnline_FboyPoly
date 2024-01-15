$(document).ready(function () {





    $('body').on('click', '#btn-openfilter-voucher', function () {
        $('#modal-filter-voucher').modal('show');

    })
    $('body').on('click', '#btn_loc', function () {
        InitPage();
        $('#modal-filter-voucher').modal('hide');

    })
    $('body').on('click', '#btn_clear', function () {
        $('#ft_TUNGAY_tu').val('');
        $('#ft_kethuc_tu').val('');
        $('#ft_TUNGAY_den').val('');
        $('#ft_kethuc_den').val('');
        $('#ft_trangthai').val('');
    })


    $('body').on('click', '#btn_xoa', function () {
        let id = $(this).attr('data-id')
        $.get('/Xoa_Voucher/' + id, function (re) {
            alert(re.message);
            window.location.reload();
        })

    })
    $('body').on('click', '#btn_chitiet', function () {
        let id = $(this).attr('data-id')
        $.get('/Voucher-Detail/' + id, function (re) {
            if (re.status) {
                setdata(re.data);
                $('#pp_modal').modal('show');
            }
            else {
                alert(re.message)
            }
        })

    })
    $('body').on('click', '#btn_addnew', function () {
        setdata('');
        $('#pp_modal').modal('show');

    })
    $('body').on('click', '#btn_save', function () {
        let id = $('#btn_id').val();
        if ($('#btn_id').val() == null || $('#btn_id').val() == undefined || $('#btn_id').val() == '')
        {
            $.post('/Add-Voucher', { vc: getdata() }, function (re) {
                if (re.status) {
                    alert(re.message);
                    $('#pp_modal').modal('hide');
                    window.location.reload();
                }
                else {
                    alert(re.message);
                }
            })
        }
        else {
            $.post('/Update-Voucher/'+id, { vc: getdata() }, function (re) {
                if (re.status) {
                    alert(re.message);
                    $('#pp_modal').modal('hide');
                    window.location.reload();
                }
                else {
                    alert(re.message);
                }
            })
        }
    })

});

function getBindFormFilter() {
    return {
        NTTU_tu: $('#ft_TUNGAY_tu').val(),
        NTDEN_tu: $('#ft_kethuc_tu').val(), 
        NTTU_den: $('#ft_TUNGAY_den').val(), 
        NTDEN_den: $('#ft_kethuc_den').val(), 
        Status: $('#ft_trangthai').val(),
        _search: $('#btn_search').val(),
    }
}

function InitPage() {

    $.post('/load-data-voucher', { filter: getBindFormFilter() }, function (re) {

        if (re.status) {

            let header = `

            <thead>
                <tr>

                    <th style="text-align : center;">
                        Tên Voucher
                    </th>
                    <th style="text-align : center;">
                        Chú Thích
                    </th>
                    <th style="text-align : center;">
                        Ngày Bắt Đầu
                    </th>
                    <th style="text-align : center;">
                        Ngày Kết Thúc
                    </th>
                    <th style="text-align : center;">
                        Giảm Giá(%)
                    </th>
                    <th style="text-align : center;">
                        Giảm Giá(Đồng)
                    </th>
                    <th style="text-align : center;">
                        Trạng Thái
                    </th>
                    <th></th>
                </tr>
            </thead>
`;

            let body = '';
            re.data.map(row => {

                let tr = `
        <tr>
            <td>${row.name}</td>
            <td>${row.code}</td>
            <td>${row.quantity}</td>
            <td>${row.description}</td>
            <td>${row.startDate}</td>
            <td>${row.endDate}</td>
            <td>${row.discountCondition}</td>
            <td>${row.discountAmount}</td>
            <td>${row.status}</td>
            <td>
            <div class="d-flex justify-content-center gap-1 flex-wrap">
            <button type="button" class="btn btn-sm btn-primary">Update</button>
            <button type="button" class="btn btn-sm btn-primary">Detail</button>
            <div>
            </td>



        </tr>
`;
                body += tr;

            });
          



            $('#table-voucher').empty();
            $('#table-voucher').append(header);
            $('#table-voucher').append(body);


        } else {
            alert(re.message);
        }

    })

}

function getdata() {

    return {
        VoucherID: $('#btn_id').val(),
        Name: $('#btn_Name').val(),
        code: $('#btn_code').val(),
        Quantity: $('#btn_Quantity').val(),
        Description: $('#btn_Description').val(),
        StartDate: moment($('#btn_StartDate').val()).format('YYYY-MM-DD'),
        EndDate: moment($('#btn_EndDate').val()).format('YYYY-MM-DD'),
        DiscountCondition: $('#btn_DiscountCondition').val(),
        DiscountAmount: $('#btn_DiscountAmount').val(),
        Status: $('#btn_Status').val()
    }

}
function setdata(data) {
    if (data == null || data == '' || data == undefined) {
        $('#btn_id').val('');
        $('#btn_Name').val('');
        $('#btn_code').val('');
        $('#btn_Quantity').val('');
        $('#btn_Description').val('');
        $('#btn_StartDate').val(moment().format('YYYY-MM-DD')); 
        $('#btn_EndDate').val(moment().format('YYYY-MM-DD'));
        $('#btn_DiscountCondition').val('');
        $('#btn_DiscountAmount').val('');
        $('#btn_Status').val('');
    }
    else {
        $('#btn_id').val(data.voucherID);
        $('#btn_Name').val(data.name);
        $('#btn_code').val(data.code);
        $('#btn_Quantity').val(data.quantity);
        $('#btn_Description').val(data.description);
        $('#btn_StartDate').val(moment(data.startDate).format('YYYY-MM-DD')); 
        $('#btn_EndDate').val(moment(data.endDate).format('YYYY-MM-DD'));
        $('#btn_DiscountCondition').val(data.discountCondition);
        $('#btn_DiscountAmount').val(data.discountAmount);
        $('#btn_Status').val(data.status);
    }
}
