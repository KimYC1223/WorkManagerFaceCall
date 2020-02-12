let callCheckBox = document.getElementById('call')
let callerIp = document.getElementById('CallerIP')
let callBtn = document.getElementById('callBtn')
let hangUpBtn = document.getElementById('hangUpBtn')
let callerIpAddress = '';

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
      if (jsonMsg.CallState == 1){
        callCheckBox.checked = true;
        callerIp.innerHTML=jsonMsg.CallerIP
        callerIpAddress = jsonMsg.CallerIP
      } else {
        callCheckBox.checked = false;
        callerIp.innerHTML='&nbsp;'
        callerIpAddress = ''
      }
  	},error: function(msg) {			// 실패시
      console.log(msg);
    }
  });
}

let connect= () => {
  jQuery.ajax({
  	type:'GET',						// POST 방식으로
  	url: '/Connecting.jsp',		// saveVideo.php로 전송
  	processData:false,					// 기본 설정
  	contentType: false,					// 기본 설정
  	data: '',							// FormData 전송
  	success: function(msg) {			// 성공시
      if (callerIpAddress != '') {
        location.href=`/callingPage.jsp?ip=${callerIpAddress}`
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

callBtn.addEventListener('click', () =>{
  connect()
})

setInterval(getState,500);
