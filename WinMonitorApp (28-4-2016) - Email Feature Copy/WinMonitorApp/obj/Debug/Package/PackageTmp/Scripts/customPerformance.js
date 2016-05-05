$(function () {
    $('#container').highcharts({
        chart: {
            type: 'bar'
        },
        title: {
            text: 'Components Stacked column chart'
        },
        xAxis: {
            categories: ['Component 1', 'Component 2']
        },
        yAxis: {
            min: 0,
            max: 24,
            title: {
                text: 'Hours in the day'
            }
        },
        legend: {
            reversed: true
        },
        tooltip: {
            pointFormat: '<span style="color:{series.color}">{series.name}</span>: <b>{point.y}</b> ({point.percentage:.0f}%)<br/>',
            shared: true
        },
        plotOptions: {
            series: {
                stacking: 'percent'
            },
        },
        series: [{
            name: 'Operational',
            data: [5, 3]
        }, {
            name: 'Performance Degradation',
            data: [2, 2]
        },
        {
            name: 'Service Disruption',
            data: [2, 2]
        }]
    });
});