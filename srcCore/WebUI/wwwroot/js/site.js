Chart.defaults.global.animation.duration = 0;
Chart.defaults.global.animation.easing = 'linear';

function testInit(hubConnection) {
    var config = {
        content: [{
            type: 'column',
            content: [{
                type: 'row',
                content: [{
                    type: 'component',
                    componentName: 'testComponent',
                    componentState: { label: 'A' }
                }, {
                    type: 'component',
                    componentName: 'testComponent',
                    componentState: { label: 'B' }
                }, {
                    type: 'component',
                    componentName: 'testComponent',
                    componentState: { label: 'C' }
                }]
            }, {
                type: 'row',
                content: [{
                    type: 'component',
                    componentName: 'testComponent',
                    componentState: { label: 'D' }
                }, {
                    type: 'component',
                    componentName: 'testComponent',
                    componentState: { label: 'E' }
                }, {
                    type: 'component',
                    componentName: 'testComponent',
                    componentState: { label: 'F' }
                }]
            }]
        }]
    }

    var myLayout = new GoldenLayout(config);
    myLayout.registerComponent('testComponent', function (container, componentState) {
        container.getElement().html('<canvas id="canvas' + componentState.label + '" width="300" height="75"></canvas>');
    });

    myLayout.init();

    var ctxA = document.getElementById('canvasA');
    var ctxB = document.getElementById('canvasB');
    var ctxC = document.getElementById('canvasC');
    var ctxD = document.getElementById('canvasD');
    var ctxE = document.getElementById('canvasE');
    var ctxF = document.getElementById('canvasF');
    var chartContent = {
        type: 'line',
        data: {
            labels: [
                1
            ],
            datasets: [{
                label: "throttle",
                data: [],
                borderColor: Chart.helpers.color('rgb(75, 192, 192)').alpha(1.0).rgbString(),
                fill: false,
                lineTension: 0,
                pointRadius: 0,
                animation: {
                    duration: 0
                },
                hover: {
                    animationDuration: 0
                },
                responsiveAnimationDuration: 0,
                elements: {
                    line: {
                        tension: 0
                    }
                },
                min: 0,
                max: 100,
                minRotation: 0,
                maxRotation: 0,
                showLine: true,
                cubicInterpolationMode: "monotone"
            }]
        },
        options: {
        }
    };
    window.myChartA = new Chart(ctxA, chartContent);
    window.myChartB = new Chart(ctxB, chartContent);
    window.myChartC = new Chart(ctxC, chartContent);
    window.myChartD = new Chart(ctxD, chartContent);
    window.myChartE = new Chart(ctxE, chartContent);
    window.myChartF = new Chart(ctxF, chartContent);

    var i = 1;
    hubConnection.on("ReceiveMessage", function (value) {
        addData(window.myChartA, i, value);
        addData(window.myChartB, i, value);
        addData(window.myChartC, i, value);
        addData(window.myChartD, i, value);
        addData(window.myChartE, i, value);
        addData(window.myChartF, i, value);
        i++;
    })
}

function addData(myChart, i, value) {
    myChart.data.labels.push(i);
    myChart.data.datasets[0].data.push({ x: i, y: value });
    if (myChart.data.labels.length > 1500) {
        myChart.data.labels.shift();
        myChart.data.datasets[0].data.shift();
    }
    myChart.update();
}