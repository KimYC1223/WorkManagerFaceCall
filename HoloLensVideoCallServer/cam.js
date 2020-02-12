function getQuerystring(paramName){ var _tempUrl = window.location.search.substring(1);
  var _tempArray = _tempUrl.split('&');
  for(var i = 0; _tempArray.length; i++) {
    var _keyValuePair = _tempArray[i].split('=');
    if(_keyValuePair[0] == paramName){
      return _keyValuePair[1];
    }
  }
}

let target = getQuerystring(('ip'))
console.log(target)

const windows = document.getElementById('windows');
windows.src="https://"+target+
    "/api/holographic/stream/live_high.mp4?holo=true&pv=true&mic=true&loopback=true";

let getState = () => {
  jQuery.ajax({
  	type:'GET',						// POST 방식으로
  	url: '/CallState.jsp',		// saveVideo.php로 전송
  	processData:false,					// 기본 설정
  	contentType: false,					// 기본 설정
  	data: '',							// FormData 전송
  	success: function(msg) {			// 성공시
      let jsonMsg = JSON.parse(msg)
      console.log(jsonMsg.CallState)
      if (jsonMsg.CallState == 0){
        location.href="/"
      }
  	},error: function(msg) {			// 실패시
      console.log(msg);
    }
  });
}

let hangUp= () => {
  jQuery.ajax({
  	type:'GET',						// POST 방식으로
  	url: '/HangUp.jsp',		// saveVideo.php로 전송
  	processData:false,					// 기본 설정
  	contentType: false,					// 기본 설정
  	data: '',							// FormData 전송
  	success: function(msg) {			// 성공시
      console.log('hangUp')
  	},error: function(msg) {			// 실패시
      console.log(msg);
    }
  });
}

hangUpBtn.addEventListener('click',() =>{
  hangUp()
})

setInterval(getState,500);
