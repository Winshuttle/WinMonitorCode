//Defining three coloured table division bars
var OperationalDiv = '<div style="background-color:green; width:25%; height:10px; float:left;"></div>';
var PerformanceDegradationDiv = '<div style="background-color:orange; width:25%; height:10px; float:left;"></div>';
var ServiceDisruptionDiv = '<div style="background-color:red; width:50%; height:10px; float:left;"></div>';


function functionbtnHistory() {
    $('#tablePerformance tbody').remove();
    var rowHistory = '<tr><td>Timeline</td><td>Day-1</td><td>Day-2</td><td>Day-3</td><td>Day-4</td><td>Day-5</td></tr>';
    $('#tablePerformance').append(rowHistory);


}

function functionbtnRecent() {
    $('#tablePerformance tbody').remove();
    var rowRecent = '<tr><td>Timeline</td><td>-6h</td><td>-12h</td><td>-18h</td><td>-24h</td></tr>';
    $('#tablePerformance').append(rowRecent);

    //an ajax call here

    $('#tablePerformance').append('<tr><td>Communication</td><td colspan="4">' + OperationalDiv + PerformanceDegradationDiv + ServiceDisruptionDiv + '</td></tr>');
    $('#tablePerformance').append('<tr><td>Processor</td><td colspan="4">' + OperationalDiv + PerformanceDegradationDiv + ServiceDisruptionDiv + '</td></tr>');
    $('#tablePerformance').append('<tr><td>Shuttler</td><td colspan="4">' + OperationalDiv + PerformanceDegradationDiv + ServiceDisruptionDiv + '</td></tr>');
    $('#tablePerformance').append('<tr><td>Management Server</td><td colspan="4">' + OperationalDiv + PerformanceDegradationDiv + ServiceDisruptionDiv + '</td></tr>');


}