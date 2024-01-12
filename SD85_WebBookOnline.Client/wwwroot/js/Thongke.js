

$(document).ready(function () {

    InitPage()
    $('#tittle').text('Thống kê tổng doanh thu')

    $('body').on('click', '#btn_loc', function () {
        if ($('#ft_TUNGAY_tu').val() == '' &&
            $('#ft_kethuc_den').val() == '' &&
            $('#btn_search').val() == ''
        ) {
            alert('Bạn cần ít nhất 1 điều kiện')
            return;

        }

        var ngay_tu = '';
        var ngay_den = '';
        var ten_sach = '';
        var ngaytu = moment($('#ft_TUNGAY_tu').val()).format('DD-MM-YYYY');
        var ngayden = moment($('#ft_kethuc_den').val()).format('DD-MM-YYYY');
        var tensach = $('#btn_search').val();
        if ($('#btn_search').val() != '') {
            ten_sach = (' sách ' + tensach);
        }
        if ($('#ft_TUNGAY_tu').val() != '') {
            ngay_tu = (' từ ngày ' + ngaytu);
        }
        if ($('#ft_kethuc_den').val() != '') {
            ngay_den = (' đến ngày ' + ngayden);
        }
        InitPage3();
        $('#tittle').text('Thống kê doanh thu' + ten_sach + ngay_tu + ngay_den)
        $('#thongke-options').val('')
        $('#modal-filter').modal('hide');

    })
    $('body').on('change', '#thongke-options', function () {

        if ($('#thongke-options').val() == 1) {
            $('#tittle').text('Thống kê tổng doanh thu')
            InitPage()
        }
        if ($('#thongke-options').val() == 2) {
            $('#tittle').text('Thống kê doanh thu ngày hôm nay')
            InitPage2()
        }
    });



    $('#btn-openfilter').on('click', function () {
        $('#ft_TUNGAY_tu').val(moment().format('YYYY-MM-DD'));

        $('#ft_kethuc_den').val(moment().format('YYYY-MM-DD'));
        $('#btn_search').val('');
        $('#modal-filter').modal('show')
    })
    $('#btn_exit').on('click', function () {

        $('#modal-filter').modal('hide')
    })
    $('body').on('click', '#btn_clear', function () {
        $('#ft_TUNGAY_tu').val(moment().format('YYYY-MM-DD'));

        $('#ft_kethuc_den').val(moment().format('YYYY-MM-DD'));
        $('#btn_search').val('');
    })
})



function getBindFormFilter() {
    return {
        NTTU_tu: $('#ft_TUNGAY_tu').val() != '' ? $('#ft_TUNGAY_tu').val() : '',
        NTDEN_den: $('#ft_kethuc_den').val() != '' ? $('#ft_kethuc_den').val() : '',
        _search: $('#btn_search').val(),
    }
}
function InitPage() {
    $.get('/GetThongKe', function (re) {
        if (re.status) {
            let header = `
                <thead>
                    <tr>
                        <th>Tên sách</th>
                        <th>Tổng số sách bán được</th>
                        <th>Tổng doanh thu</th>
                        <th>Lợi nhuận</th>
                        <th>Chi phí gốc</th>
                        <th>Số sách còn lại</th>
                    </tr>
                </thead>`;
            let body = '';
            re.data.forEach(row => {
                let tr = `<tr>
                           
                            <td>${row.tensach}</td>
                            <td>${row.tongSoSachBanDuoc}</td>
                            <td>${row.tongDoanhThusach}</td>
                            <td>${row.loiNhuansach}</td>
                            <td>${row.chiPhiGocsach}</td>
                            <td>${row.soSachConLai != 0 ? row.soSachConLai : ''}</td>
                          </tr>`;
                body += tr;
            });

            // Cập nhật hoặc tạo mới table
            if ($('#table-thongke').length > 0) {
                $('#table-thongke').empty();
                $('#table-thongke').append(header);
                $('#table-thongke').append(body);
            }
        } else {
            alert(re.message);
        }
    });
}
function InitPage2() {
    $.get('/ThongkeNgay', function (re) {
        if (re.status) {
            let header = `
                <thead>
                    <tr>
                       
                        <th>Tên sách</th>
                        <th>Tổng số sách bán được</th>
                        <th>Tổng doanh thu</th>
                        <th>Lợi nhuận</th>
                        <th>Chi phí gốc</th>
                        <th>Số sách còn lại</th>
                    </tr>
                </thead>`;
            let body = '';
            re.data.forEach(row => {
                let tr = `<tr>
                         
                            <td>${row.tensach}</td>
                            <td>${row.tongSoSachBanDuoc}</td>
                            <td>${row.tongDoanhThusach}</td>
                            <td>${row.loiNhuansach}</td>
                            <td>${row.chiPhiGocsach}</td>
                            <td>${row.soSachConLai != 0 ? row.soSachConLai : ''}</td>
                          </tr>`;
                body += tr;
            });

            // Cập nhật hoặc tạo mới table
            if ($('#table-thongke').length > 0) {
                $('#table-thongke').empty();
                $('#table-thongke').append(header);
                $('#table-thongke').append(body);
            }
        } else {
            alert(re.message);
        }
    });

}
function InitPage3() {
    $.post('/load-data-thongke', { filter: getBindFormFilter() }, function (re) {
        if (re.status) {
            let header = `
                <thead>
                    <tr>
                       
                        <th>Tên sách</th>
                        <th>Tổng số sách bán được</th>
                        <th>Tổng doanh thu</th>
                        <th>Lợi nhuận</th>
                        <th>Chi phí gốc</th>
                        <th>Số sách còn lại</th>
                    </tr>
                </thead>`;
            let body = '';
            re.data.forEach(row => {
                let tr = `<tr>
                         
                            <td>${row.tensach}</td>
                            <td>${row.tongSoSachBanDuoc}</td>
                            <td>${row.tongDoanhThusach}</td>
                            <td>${row.loiNhuansach}</td>
                            <td>${row.chiPhiGocsach}</td>
                            <td>${row.soSachConLai != 0 ? row.soSachConLai : ''}</td>
                          </tr>`;
                body += tr;
            });

            // Cập nhật hoặc tạo mới table
            if ($('#table-thongke').length > 0) {
                $('#table-thongke').empty();
                $('#table-thongke').append(header);
                $('#table-thongke').append(body);
            }
        } else {
            alert(re.message);
        }
    });
}

function generateExcel() {
    $.get('/GetThongKe', function (re) {
        if (re.status) {
            var data = re.data;

            // Tạo một đối tượng JSON để lưu trữ dữ liệu
            var jsonData = [];

            // Lặp qua dữ liệu để chuyển đổi thành định dạng phù hợp để tạo Excel
            data.forEach(function (item) {
                var row = {
                    'Tên sách': item.tensach,
                    'Tổng số sách bán được': item.tongSoSachBanDuoc,
                    'Tổng doanh thu': item.tongDoanhThusach,
                    'Lợi nhuận': item.loiNhuansach,
                    'Chi phí gốc': item.chiPhiGocsach,
                    'Số sách còn lại': item.soSachConLai != 0 ? item.soSachConLai : '',
                };
                jsonData.push(row);
            });

            // Tạo một đối tượng workbook từ dữ liệu JSON
            var workbook = XLSX.utils.book_new();
            var ws = XLSX.utils.json_to_sheet(jsonData);

            // Thêm sheet vào workbook
            XLSX.utils.book_append_sheet(workbook, ws, 'ThongKeSheet');

            // Tạo buffer để lưu trữ dữ liệu Excel
            var buffer = XLSX.write(workbook, { bookType: 'xlsx', type: 'array' });

            // Tạo blob từ buffer
            var blob = new Blob([buffer], { type: 'application/octet-stream' });

            // Tạo URL từ blob và tạo ra thẻ <a> để tải xuống
            var url = window.URL.createObjectURL(blob);
            var a = document.createElement('a');
            a.href = url;
            a.download = 'ThongKe.xlsx';

            // Thêm thẻ <a> vào body và kích hoạt sự kiện click để tải xuống
            document.body.appendChild(a);
            a.click();

            // Xóa thẻ <a> sau khi tải xuống
            document.body.removeChild(a);
        } else {
            alert(re.message);
        }
    });
}
