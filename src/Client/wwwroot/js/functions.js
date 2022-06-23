// Global ChartJs object to keep track of when needing to refresh chart
var histogramChart = null;

// Send data to chart
function setupChart(id, config) {
    var ctx = document.getElementById(id).getContext('2d');
    histogramChart = new Chart(ctx, config);
}

// Chart instance must be cleared before recreating new chart (when switching artists)
function resetCanvas() {
    histogramChart.destroy();
}

function downloadFileToBrowser(fileName, fileContent) {
    var link = document.createElement('a');
    link.download = fileName;
    link.href = "data:text/csv;charset=utf-8," + encodeURIComponent(fileContent)
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
}