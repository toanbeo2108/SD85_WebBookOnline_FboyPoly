
    $(document).ready(function () {
        var myBarChart;
        $('#yearSelector').val('');
        $('#monthSelector').val('');
    // Khi trang được tải, gọi fetchDataAndDrawChart với giá trị ban đầu của ComboBox là null
    fetchDataAndDrawChart(null, null);

    // Lắng nghe sự kiện thay đổi trên ComboBox năm và tháng
    $("#yearSelector, #monthSelector").change(function () {
            // Lấy giá trị năm và tháng được chọn
            var selectedYear = $("#yearSelector").val();
    var selectedMonth = $("#monthSelector").val();

    // Lọc dữ liệu và vẽ lại biểu đồ cho năm và tháng mới
    fetchDataAndDrawChart(selectedYear, selectedMonth);
        });

    function fetchDataAndDrawChart(selectedYear, selectedMonth) {
        $.ajax({
            type: "GET",
            url: "/GetChart",
            data: { selectedYear: selectedYear, selectedMonth: selectedMonth },
            contentType: "application/json",
            dataType: "json",
            success: function (result) {
                if (result.status) {
                    var data = result.data;
                    var labels, values;

                    if (selectedMonth) {
                        // Nếu chọn tháng, trục X sẽ là ngày đầu tiên đến ngày cuối cùng của tháng đó
                        var daysInMonth = moment(selectedYear + "-" + selectedMonth, "YYYY-MM").daysInMonth();
                        labels = Array.from({ length: daysInMonth }, (_, i) => i + 1);
                    } else {
                        // Nếu không chọn tháng, trục X sẽ là từ tháng 1 đến tháng 12
                        labels = Array.from({ length: 12 }, (_, i) => i + 1);
                    }

                    values = data.map(function (item) {
                        return item.totalRevenue;
                    });

                    // Vẽ hoặc cập nhật biểu đồ cột
                    if (myBarChart) {
                        updateBarChart(myBarChart, labels, values, selectedMonth);
                    } else {
                        myBarChart = drawBarChart(labels, values, selectedMonth);
                    }
                } else {
                    console.error(result.message);
                }
            },
            error: function (error) {
                console.error("Error fetching data: " + error.statusText);
            }
        });
        }

    function drawBarChart(labels, values, selectedMonth) {
            var ctx = document.getElementById('myBarChart').getContext('2d');
    var chart = new Chart(ctx, {
        type: 'bar',
    data: {
        labels: labels,
    datasets: [{
        label: 'Tổng doanh thu',
    data: values,
    backgroundColor: 'rgba(75, 192, 192, 0.2)',
    borderColor: 'rgba(75, 192, 192, 1)',
    borderWidth: 1
                    }]
                },
    options: {
        scales: {
        y: {
        beginAtZero: true,
    title: {
        display: true,
    text: 'Doanh thu'
                            }
                        },
    x: {
        title: {
        display: true,
    text: '' 
                            }
                        }
                    }
                }
            });

    return chart;
        }

    function updateBarChart(chart, labels, values, selectedMonth) {
        chart.data.labels = labels;
    chart.data.datasets[0].data = values;
    chart.update();
        }
    });

