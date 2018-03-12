var player;

/*
function onYouTubeIframeAPIReady() {
    alert('yt loaded');
    player = new YT.Player('ytplayer', {
        events: {
            'onReady': onPlayerReady,
            'onStateChange': onPlayerStateChange
        }
    });
    console.log(player);
}

function onPlayerReady() {
    alert('player ready');
}

function onPlayerStateChange() {
    alert('player state change');
}

*/

function onYouTubeIframeAPIReady() {
    alert('yt ready');
    player = new YT.Player('video-placeholder', {
        width: 600,
        height: 400,
        videoId: 'QOCaacO8wus',
        playerVars: {
            autoplay: 1,
            playlist: '',
            controls: 0,
            enablejsapi: 1,
            //end: 10000
            iv_load_policy: 3, // hide anotation
            rel: 0,
            showinfo: 0

        },
        events: {
            onReady: initialize
        }
    });
}


function initialize(){
	console.log('fucing initialized');
}


function pause() {
    player.pauseVideo();
}

function play() {
    player.playVideo();
}

function getVid() {
    var vis = document.getElementsByTagName('video');
    alert(vis.length);
}

