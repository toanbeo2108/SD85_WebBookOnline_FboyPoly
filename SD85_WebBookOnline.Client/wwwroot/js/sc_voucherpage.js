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