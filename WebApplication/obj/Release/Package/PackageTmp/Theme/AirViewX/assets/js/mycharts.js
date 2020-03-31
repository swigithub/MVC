
// CUSTOMIZE CHART FOR MULTISTACK 
/////////////////////////////////
var justifyColumns = function (chart) {
    var categoriesWidth = chart.plotSizeX / (1 + chart.xAxis[0].max - chart.xAxis[0].min),
        distanceBetweenColumns = 0,
        each = Highcharts.each,
        sum, categories = chart.xAxis[0].categories,
        number;
    for (var i = 0; i < categories.length; i++) {
        sum = 0;
        each(chart.series, function (p, k) {
            if (p.visible) {
                each(p.data, function (ob, j) {
                    if (ob.category == categories[i]) {
                        sum++;
                    }
                });
            }
        });
        distanceBetweenColumns = categoriesWidth / (sum + 1);
        number = 1;
        each(chart.series, function (p, k) {
            if (p.visible) {
                each(p.data, function (ob, j) {
                    if (ob.category == categories[i]) {
                        ob.graphic.element.x.baseVal.value = i * categoriesWidth + distanceBetweenColumns * number - ob.pointWidth / 2;
                        number++;
                    }
                });
            }
        });
    }
};

var justifyDataLabels = function (chart) {
    $.each(chart.series, function (j, k) {
        $.each(k.data, function (i, point) {
            point.dataLabel.attr({
                'transform': 'translate(' + (chart.plotWidth - point.plotY) + ',' + (chart.plotHeight - point.graphic.element.x.baseVal.value - 16) + ')'
            });
        });
    });
};

$(function () {
    $('#placeholder').highcharts({
        chart: {
            type: 'column',
        },
        title: false,
        xAxis: {
            categories: ['Active', 'Planned', 'Scheduled', 'Undefinded', 'Cancellled', 'Completed'],
            labels: {
                enabled: true,
            }
        }, legend: { enabled: false },
        yAxis: {
            title: false,
            labels: {
                formatter: function() {
                    return this.value + "w";
                }
            },
            stackLabels: {
                enabled: false,
                style: {
                    fontWeight: 'normal',
                    color: (Highcharts.theme && Highcharts.theme.textColor) || 'gray'
                }
            },

        },
        tooltip: {
            formatter: function () {
                return '<b>' + ToString(parseFloat(this.y)) + '</b>  days  ' + this.series.name + ' <b>' + ($(this)[0].series.stackKey).split(",")[1] + '</b>';
            }
        },
        plotOptions: {

            column: {
                stacking: 'hanging',
            },
            series: {
                pointWidth: 15
            }

        },

        series:
        [
            {
                name: 'Task 1',
                data: [{ x: 0, y: 7, duration: 7 }], stack: '5G Roll Out', color: '#008edd'
            },
            {
                name: 'Task 2',
                data: [{ x: 0, y: 2, duration: 3 }], stack: '5G Roll Out', color: '#008edd'
            }, {
                name: 'Task 3',
                data: [{ x: 0, y: 5, duration: 7 }], stack: 'LTE Enhancement', color: '#008edd'
            }, {
                name: 'Task 4',
                data: [{ x: 0, y: 2, duration: 2 }], stack: 'LTE Enhancement', color: '#008edd'
            }, {
                name: 'Task 5',
                data: [{ x: 0, y: 1, duration: 5 }], stack: 'ZTM Deployement', color: '#ECECEC'
            }, {
                name: 'Task 6',
                data: [{ x: 0, y: 1, duration: 5 }], stack: 'ZTM Deployement', color: '#008edd'
            }, {
                name: 'Task 7',
                data: [{ x: 0, y: 5, duration: 1 }], stack: 'ZTM Deployement', color: '#008edd'
            }, {
                name: 'Task 8',
                data: [{ x: 0, y: 4, duration: 4 }], stack: 'ZTM Deployement', color: '#008edd'
            }, {
                name: 'Task 9',
                data: [{ x: 0, y: 4, duration: 4 }], stack: 'ZTM Deployement', color: '#008edd'
            }, {
                name: 'Task 10',
                data: [{ x: 0, y: 5, duration: 2 }], stack: 'ZTM Deployement', color: '#008edd'
            }, {
                name: 'Task 11',
                data: [{ x: 0, y: 5, duration: 5 }], stack: 'Sprint-11', color: '#008edd'
            }, {
                name: 'Task 12',
                data: [{ x: 0, y: 5, duration: 5 }], stack: 'Sprint-11', color: '#008edd'
            }, {
                name: 'Task 13',
                data: [{ x: 1, y: 2, duration: 5 }], stack: '3g-Service', color: '#ECECEC'
            }, {
                name: 'Task 14',
                data: [{ x: 1, y: 5, duration: 5 }], stack: '3g-Service', color: 'orange'
            }, {
                name: 'Task 15',
                data: [{ x: 2, y: 0, duration: 0 }], stack: '4g-Service', color: '#ECECEC'
            }, {
                name: 'Task 16',
                data: [{ x: 2, y: 2, duration: 0 }], stack: '4g-Service', color: 'yellow'
            }, {
                name: 'Task 17',
                data: [{ x: 2, y: 2, duration: 0 }], stack: '4g-Service', color: 'yellow'
            }, {
                name: 'Task 18',
                data: [{ x: 2, y: 7, duration: 0 }], stack: '4g-Service1', color: 'yellow'
            }, {
                name: 'Task 19',
                data: [{ x: 2, y: 7, duration: 0 }], stack: '4g-Service1', color: 'yellow'
            }, {
                name: 'Task 20',
                data: [{ x: 2, y: 7, duration: 0 }, ], stack: '4g-Service1', color: 'yellow'
            }, {
                name: 'Task 21',
                data: [{ x: 3, y: 4, duration: 4 }], stack: '4g-Service2', color: 'gray'
            }, {
                name: 'Task 22',
                data: [{ x: 3, y: 8, duration: 8 }], stack: '4g-Service3', color: '#ECECEC'
            }, {
                name: 'Task 23',
                data: [{ x: 3, y: 5, duration: 5 }], stack: '4g-Service3', color: 'gray'
            }, {
                name: 'Task 24',
                data: [{ x: 3, y: 8, duration: 8 }], stack: '4g-Service3', color: 'gray'
            }
        ]
    });

});
function ToString(num) {
    num = isNaN(num) || num === '' || num === null ? 0.00 : num;
    return parseFloat(num).toFixed(0);
}

 
//summaryIssuesPieChart 
Highcharts.chart('summaryIssuesPieChart', {
    chart: {
        plotBackgroundColor: null,
        plotBorderWidth: null,
        plotShadow: false,
        type: 'pie'
    },
    title: false,
    tooltip: {
        pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
    },
    plotOptions: {
        pie: {
            allowPointSelect: true,
            cursor: 'pointer',
            dataLabels: {
                enabled: false
            },
            showInLegend: true
        }
    },
    series: [{
        name: 'Brands',
        colorByPoint: true,
        data: [{
            name: 'Projects',
            y: 61,
            sliced: true,
            selected: true,
            color: '#fc6701'
        }, {
            name: 'O & M',
            y: 41,
            color: '#f9d84b'
        }]
    }]
});

//summary Work Orders PieChart 
Highcharts.chart('summaryWorkOrderPie', {
    chart: {
        plotBackgroundColor: null,
        plotBorderWidth: null,
        plotShadow: false,
        type: 'pie'
    },
    title: false,
    tooltip: {
        pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
    },
    plotOptions: {
        pie: {
            allowPointSelect: true,
            cursor: 'pointer',
            dataLabels: {
                enabled: false
            },
            showInLegend: true,
            legend: {
                itemStyle: {
                    fontSize: '10px',
                }
            }
        }
    },
    series: [{
        name: 'TicketsIssues',
        colorByPoint: true,
        data: [{
            name: 'In-Progress',
            y: 14,
            sliced: true,
            selected: true,
            color: '#35acfe'
        }, {
            name: 'Pending Scheduled',
            y: 16,
            color: '#dd6400'
        }, {
            name: 'Pending with Issues',
            y: 20,
            color: '#a70237'
        }, {
            name: 'Scheduled',
            y: 10,
            color: '#e2c620'
        }, {
            name: 'Approved',
            y: 10,
            color: '#00a89f'
        }, {
            name: 'Drive Completed',
            y: 30,
            color: '#137f13'
        }, {
            name: 'Report Submitted',
            y: 30,
            color: '#5291c4'
        }]
    }]
});

//summary Tickets PieChart  
Highcharts.chart('summaryTicketsPieChart', {
    chart: {
        plotBackgroundColor: null,
        plotBorderWidth: null,
        plotShadow: false,
        type: 'pie'
    },
    title: false,
    tooltip: {
        pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
    },
    plotOptions: {
        pie: {
            allowPointSelect: true,
            cursor: 'pointer',
            dataLabels: {
                enabled: false
            },
            showInLegend: true
        }
    },
    series: [{
        name: 'TicketsIssues',
        colorByPoint: true,
        data: [{
            name: 'Overdue',
            y: 14,
            sliced: true,
            selected: true,
            color: '#137f13'
        }, {
            name: 'Pending',
            y: 16,
            color: '#e2c620'
        }, {
            name: 'Active',
            y: 20,
            color: '#35acfe'
        }, {
            name: 'On-Hold',
            y: 10,
            color: '#f2982c'
        }, {
            name: 'Planned',
            y: 10,
            color: '#808080'
        }, {
            name: 'Completed',
            y: 30,
            color: '#a70237'
        }]
    }]
});
 
    var istart; var iend;
    function cb2(start, end) {

        IFromDate = moment(start).format('MM/DD/YY');
        IToDate = moment(end).format('MM/DD/YY');

        $('#reportrange2 span').html(IFromDate + ' - ' + IToDate);
    }
//
    $('#reportrange2').daterangepicker({
      //  startDate: istart,
        endDate: iend,
        orientation: 'left',
        minDate: moment().add(-5, 'years'),
        maxDate: moment().add(5, 'years'),
        ranges: {
            'Today': [moment(), moment()],
            'Yesterday': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
            'Last 7 Days': [moment().subtract(6, 'days'), moment()],
            'Last 30 Days': [moment().subtract(29, 'days').subtract(1, 'days'), moment()],
            'This Month': [moment().startOf('month').add(1, 'days'), moment().endOf('month')],
            'Last Month': [moment().subtract(1, 'month').startOf('month').add(1, 'days'), moment().subtract(1, 'month').endOf('month')],
            'Custom Range': "custom"
        }
    }, cb2);

    cb2(istart, iend);



