var cvs = document.getElementById("cvs");
var ctx = cvs.getContext("2d");
var cvs_score = document.getElementById("scroe");
var ctx_score = cvs_score.getContext("2d");
var img_Bird = document.getElementById("bird");
var img_Back1 = document.getElementById("back1");
var img_Back2 = document.getElementById("back2");
var fen;
//全局变量
var g = 0.002;//重力加速度
var fly_Power = -0.6;//飞行力度
var background_speed = 3;//背景移动速度
var between = 200;
var game_over = false;
var score = 0;//分数
//全局变量结束

//onlond函数
window.onload = function () {
    $.ajax({
        url: '/hand.ashx',
        type: 'POST',
        data: {
            method: 'flappy',
            nowusername: nowusername,
        },
        success: function (outflappy) {
            outflappy = outflappy.split('`');
            var top1_name = outflappy[0];
            var top1_bird = outflappy[1];
            var top2_name = outflappy[2];
            var top2_bird = outflappy[3];
            var top3_name = outflappy[4];
            var top3_bird = outflappy[5];
            var nowbird = outflappy[6];
            fen = nowbird;
            ctx_score.font = '20px Microsoft YaHei';
            ctx_score.fillStyle = '#EE4000';
            ctx_score.fillText("第一名", 30, 18); ctx_score.fillText(top1_name, 130, 18); ctx_score.fillText(top1_bird, 280, 18);
            ctx_score.fillText("第二名", 30, 38); ctx_score.fillText(top2_name, 130, 38); ctx_score.fillText(top2_bird, 280, 38);
            ctx_score.fillText("第三名", 30, 58); ctx_score.fillText(top3_name, 130, 58); ctx_score.fillText(top3_bird, 280, 58);
            ctx_score.fillText("当前用户", 30, 78); ctx_score.fillText(nowusername, 130, 78); ctx_score.fillText(nowbird, 280, 78);
        },
        error: function () { alert('error flappy'); }
    });

}
//onlond函数结束

//Bird的构造函数
var Bird = function (img, x, y, speed, ctx) {
    this.img = img;
    this.x = x;
    this.y = y;
    this.speed = speed;
    this.ctx = ctx;
}

Bird.prototype.draw = function () {
    this.ctx.drawImage(this.img, this.x, this.y, 48, 42);
}
Bird.prototype.update = function (t) {
    this.speed = g * t + this.speed;
    this.y += Math.floor(0.5 * g * t * t + this.speed * t);
}
//Bird的构造函数结束

//背景的构造函数
var backGround = function (img1, img2, x, y, speed, ctx) {
    this.img1 = img1;
    this.img2 = img2;
    this.x = x;
    this.y = y;
    this.speed = speed;
    this.ctx = ctx;
}
backGround.prototype.draw = function () {
    this.ctx.drawImage(this.img1, this.x, this.y, 1200, 600);
    this.ctx.drawImage(this.img2, this.x + 1200, this.y, 1200, 600);
}
backGround.prototype.update = function () {
    if (this.x == -1200) {
        this.x = 0;
    }
    this.x = this.x - background_speed;
}
//背景的构造函数结束

//木桶的构造函数
var bucket = function (x, long, ctx) {
    this.x = x;
    this.long = long;
    this.ctx = ctx;
}
bucket.prototype.draw = function () {
    ctx.beginPath();
    ctx.fillStyle = "#F75000";/*设置填充颜色*/
    //上桶
    ctx.fillRect(this.x, 0, 50, this.long);
    ctx.fillRect(this.x - 5, this.long, 60, 10);
    //下桶
    ctx.fillRect(this.x - 5, this.long + 10 + between, 60, 10);
    ctx.fillRect(this.x, this.long + 10 + between + 10, 50, 600 - (this.long + 10 + between + 10));
    ctx.closePath();//可选步骤，关闭绘制的路径
    ctx.stroke(); //填充
}

bucket.prototype.update = function () {
    if (this.x == -300) {
        this.x = 1200;
        this.long = Math.floor(Math.random() * 300 + 50);
        // console.log(this.long);
    }
    this.x = this.x - background_speed - 2;
}
bucket.prototype.hit = function (bx, by) {
    if ((bx + 48 > this.x - 5 && bx + 48 < this.x + 55 && by < this.long + 10) || (bx + 48 > this.x - 5 && bx + 48 < this.x + 55 && by > this.long + 10 + between)) {
        game_over = true;//右上角
    }
    if ((bx > this.x - 5 && bx < this.x + 55 && by < this.long + 10) || (bx > this.x - 5 && bx < this.x + 55 && by > this.long + 10 + between)) {
        game_over = true;//左上角
    }
    if ((bx + 48 > this.x - 5 && bx + 48 < this.x + 55 && by + 42 < this.long + 10) || (bx + 48 > this.x - 5 && bx + 48 < this.x + 55 && by + 42 > this.long + 10 + between)) {
        game_over = true;//右下角
    }
    if ((bx > this.x - 5 && bx < this.x + 55 && by + 42 < this.long + 10) || (bx > this.x - 5 && bx < this.x + 55 && by + 42 > this.long + 10 + between)) {
        game_over = true;//左下角
    }
}

//木桶的构造函数结束

//判断得分函数
var defen = function (score) {
    ctx.font = '40px Microsoft YaHei';
    ctx.fillStyle = '#DCDCDC';
    ctx.fillText(score, 30, 70);
}
//判断得分函数结束

var preTime = Date.now();             //获取当前时间
var b = new Bird(img_Bird, cvs.width / 5, cvs.height / 8, 0.0003, ctx);//创建小鸟
var back = new backGround(img_Back1, img_Back2, 0, 0, background_speed, ctx);//创建背景
var bucket_one = new bucket(1200, Math.floor(Math.random() * 300 + 50), ctx);
var bucket_two = new bucket(1500, Math.floor(Math.random() * 300 + 50), ctx);
var bucket_three = new bucket(1800, Math.floor(Math.random() * 300 + 50), ctx);
var bucket_four = new bucket(2100, Math.floor(Math.random() * 300 + 50), ctx);
var bucket_five = new bucket(2400, Math.floor(Math.random() * 300 + 50), ctx);

//主函数
function run() {
    var now = Date.now();         //获取最新时间
    dt = now - preTime;            //获取时间间隔
    preTime = now;                  //更新当前时间
    ctx.clearRect(0, 0, 800, 600);    //清空画布
    //---------------------------------------------
    //画背景
    back.update();
    back.draw();
    //画背景结束
    bucket_one.hit(b.x, b.y);
    bucket_one.update();
    bucket_one.draw();
    bucket_two.hit(b.x, b.y);
    bucket_two.update();
    bucket_two.draw();
    bucket_three.hit(b.x, b.y);
    bucket_three.update();
    bucket_three.draw();
    bucket_four.hit(b.x, b.y);
    bucket_four.update();
    bucket_four.draw();
    bucket_five.hit(b.x, b.y);
    bucket_five.update();
    bucket_five.draw();
    //画小鸟
    b.update(dt);
    b.draw();
    //画小鸟结束

    //判断得分
    var flag = false;
    if (b.x == bucket_one.x || b.x == bucket_two.x || b.x == bucket_three.x || b.x == bucket_four.x || b.x == bucket_five.x) {
        flag = true;
    }
    if (flag == true) {
        score++;
    }
    flag = false;
    defen(score);
    // console.log(score);
    //判断得分结束

    //判断游戏结束
    if (b.y > 600 || b.y < 0) {
        game_over = true;
    }
    if (game_over == false) {
        requestAnimationFrame(run);
    }
    else {
        if (score > fen) {
            $.ajax({
                url: '/hand.ashx',
                type: 'POST',
                data: {
                    method: 'flappy_end',
                    nowusername: nowusername,
                    sc: score,
                },
                success: function (outflappy_end) {

                },
                error: function () { alert('error flappy end'); }
            });
        }
        var result = window.confirm("GAME OVER\n是否从新开始");
        if (result) {
            location.reload();
        }
    }
    //判断游戏结束
}
requestAnimationFrame(run);//首次执行run函数；
cvs.addEventListener("click", function () {
    b.speed = fly_Power;
});



//主函数结束
