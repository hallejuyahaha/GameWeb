var me = true;//判断谁先走棋用的变量
var wins = [];//赢法数组
var count = 0; //赢法索引
var over = false;//判断是否结束
var imwin = false;//我赢了？
var table = new Array(15);
for (var i = 0; i < 15; i++) {   //初始化table全为0
    table[i] = new Array(15);
}
for (var i = 0; i < table.length; i++) {
    for (var j = 0; j < table[i].length; j++) {
        table[i][j] = 0;
    }
}
//遍历赢法数组
for (var i = 0; i < 15; i++) {
    wins[i] = [];
    for (var j = 0; j < 15; j++) {
        wins[i][j] = [];
    }
}
//给赢法数组设定赢法    横线
for (var i = 0; i < 15; i++) {
    for (var j = 0; j < 11; j++) {
        for (var k = 0; k < 5; k++) {
            wins[i][j + k][count] = true;
        }
        count++;
    }
}
//竖线
for (var i = 0; i < 15; i++) {
    for (var j = 0; j < 11; j++) {
        for (var k = 0; k < 5; k++) {
            wins[j + k][i][count] = true;
        }
        count++;
    }
}
//斜线
for (var i = 0; i < 11; i++) {
    for (var j = 0; j < 11; j++) {
        for (var k = 0; k < 5; k++) {
            wins[i + k][j + k][count] = true;
        }
        count++;
    }
}
//反斜线
for (var i = 0; i < 11; i++) {
    for (var j = 14; j < 3; j--) {
        for (var k = 0; k < 5; k++) {
            wins[i + k][j - k][count] = true;
        }
        count++;
    }
}
//判断谁赢数组
var mywin = [];
var pcwin = [];
for (var i = 0; i < count; i++) {
    mywin[i] = 0;
    pcwin[i] = 0;
}

var chess = document.getElementById("chess");
var content = chess.getContext("2d");//canvas 
content.strokeStyle = "#BFBFBF";

window.onload = function () {
    $.ajax({
        url: '/hand.ashx',
        type: 'POST',
        data: {
            method: 'five',
        },
        success: function (outfive) {
            outfive = outfive.split('`');
            b.innerHTML = "名字： " + outfive[0] + " 分数 " + outfive[1];
        },
        error: function () { alert('error'); }
    });
    drawboard();
}
//画棋盘
var drawboard = function () {
    for (var i = 0; i < 15; i++) {
        content.moveTo(15, 15 + i * 30);//线的开始点
        content.lineTo(435, 15 + i * 30);//线的结束点
        content.stroke();//填充
        content.moveTo(15 + i * 30, 15);
        content.lineTo(15 + i * 30, 435);
        content.stroke();
    }
}

//每走一步
var onestep = function (i, j, BorW) {
    content.beginPath();
    content.arc(15 + i * 30, 15 + j * 30, 13, 0, 2 * Math.PI);//花园
    content.closePath();
    var gradient = content.createRadialGradient(15 + i * 30 + 2, 15 + j * 30 - 2, 13, 15 + i * 30 + 2, 15 + j * 30 - 2, 5);//渐变特效
    if (BorW) {
        gradient.addColorStop(0, "#0A0A0A");
        gradient.addColorStop(1, "#636766");
    } else {
        gradient.addColorStop(0, "#D1D1D1");
        gradient.addColorStop(1, "#F9F9F9");
    }
    content.fillStyle = gradient;
    content.fill();
}

//每点一下
chess.onclick = function (e) {
    if (over == true) {    //如果已结束就跳出
        return;
    }
    if (!me) {
        return;//如果不是我就，就跳出
    }
    var x = e.offsetX;
    var y = e.offsetY; //获取鼠标点击的坐标
    var i = Math.floor(x / 30);
    var j = Math.floor(y / 30);
    if (table[i][j] == 0) {
        onestep(i, j, me);
        table[i][j] = 1;
        for (var k = 0; k < count; k++) {
            if (wins[i][j][k] == true) {
                mywin[k]++;
                pcwin[k] = 6;
                if (mywin[k] == 5) {
                    window.alert("你赢了");
                   /* $.ajax({
                        url: '/hand.ashx',
                        type: 'POST',
                        dataType: 'text',
                        data: {
                            method: 'fivewin',
                            username: nowusername,
                        },
                        success: function (outfivewin) {

                        },
                        error: function () { alert('error'); }
                    });*/
                    over = true;
                }
            }
        }
        if (!over) {
            me = !me;
            computerAI();
        }
    }
}

var computerAI = function () {
    var myScore = [];
    var computerScore = [];
    var Max = 0;//最高评分
    var u = 0;
    var v = 0;//落子坐标
    for (var i = 0; i < 15; i++) {
        myScore[i] = [];
        computerScore[i] = [];
        for (var j = 0; j < 15; j++) {
            myScore[i][j] = 0;
            computerScore[i][j] = 0;
        }
    }
    for (var i = 0; i < 15; i++) {
        for (var j = 0; j < 15; j++) {
            if (table[i][j] == 0) {
                for (var k = 0; k < count; k++) {
                    if (wins[i][j][k]) {
                        if (mywin[k] == 1) {
                            myScore[i][j] += 200;
                        } else if (mywin[k] == 2) {
                            myScore[i][j] += 400;
                        } else if (mywin[k] == 3) {
                            myScore[i][j] += 2000;
                        } else if (mywin[k] == 4) {
                            myScore[i][j] += 10000;
                        }


                        if (pcwin[k] == 1) {
                            computerScore[i][j] += 220;
                        } else if (pcwin[k] == 2) {
                            computerScore[i][j] += 420;
                        } else if (pcwin[k] == 3) {
                            computerScore[i][j] += 2200;
                        } else if (pcwin[k] == 4) {
                            computerScore[i][j] += 20000;
                        }
                    }
                }
                if (myScore[i][j] > Max) {
                    Max = myScore[i][j];
                    u = i;
                    v = j;
                } else if (myScore[i][j] == Max) {
                    if (computerScore[i][j] > computerScore[u][v]) {
                        u = i;
                        v = j;
                    }
                }
                if (computerScore[i][j] > Max) {
                    Max = computerScore[i][j];
                    u = i;
                    v = j;
                } else if (computerScore[i][j] == Max) {
                    if (myScore[i][j] > myScore[u][v]) {
                        u = i;
                        v = j;
                    }
                }
            }
        }
    }
    onestep(u, v, false);
    table[u][v] = 2;
    for (var k = 0; k < count; k++) {
        if (wins[u][v][k]) {
            pcwin[k]++;
            mywin[k] = 6;
            if (pcwin[k] == 5) {
                window.alert("电脑赢了");
                over = true;
            }
        }
    }
    if (!over) {
        me = !me;
    }
}