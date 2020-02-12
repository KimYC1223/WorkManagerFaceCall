# HololLens Video Call Web page

![image](https://user-images.githubusercontent.com/40852277/74333146-b0a47a80-4dda-11ea-9e35-4f35b45a36a2.png)

홀로렌즈 기반의 화상통화를 위한 웹 페이지

기존 서버는 tomcat과 같은 jsp 서버로 운영이 되는듯 하지만

일단은 테스트용도로 NodeJS의 Express 모듈을 이용한 웹서버로 테스트한다.


---

##  1 . 개요

(작성중)



---

## 2 . 환경 설정

### 2.1 NodeJS 설치

[NodeJS 공식 홈페이지](https://nodejs.org/ko/)를 통해 NodeJS를 설치한다. 버전은 크게 상관 없다.



### 2.2 MySQL 설치

[MySQL 공식 홈페이지](https://dev.mysql.com/downloads/)를 통해 ```MySQL Community Server```를 다운받는다.

역시 버전은 상관 없다. MySQL 또는 다른 DB가 설치되어 있다면 해당 부분을 넘어가도 된다.

모든 설치가 완료된 후, ```C:\Program Files\MySQL\MySQL Server 8.0\bin```를 환경 변수에 추가한다.

(Mysql 기본 설치 경로가 변경된다면 그에 맞춰 바꿔준다.)



### 2.3 Data Base Init

데이터베이스 및 테이블 기본 설정을 진행한다.

cmd창을 열고, ```mysql -u root(User ID입력) -p``` 을 입력한다.

그 후, 다음을 복사 붙혀넣기 한다.

```sql
CREATE DATABASE holocalldb;
USE holocalldb;

CREATE TABLE holocall (CallState int,CallerIP char(13));
INSERT INTO holocall VALUE (0,null);

SELECT * FROM holocall;
```

다음과 같이 출력되면 잘 된 것이다.

```sql
+-----------+----------+
| CallState | CallerIP |
+-----------+----------+
|         0 | NULL     |
+-----------+----------+
1 row in set (0.01 sec)
```



### 2.4 Node module Init

웹 페이지 구동을 위해 필요한 파일들을 다운로드 해야한다.

원하는 디렉토리에서 다음과 같이 입력한다.

```git clone git@github.com:KimYC1223/WorkManagerFaceCall.git```

git clone이 완료되면,

해당 디렉토리에서 ```cd WorkManagerFaceCall\HoloLensVideoCallServer```를 입력하고

```npm install```을 입력한다.



### 2.5 MySQL Server Connection 부분 수정

```WorkManagerFaceCall\HoloLensVideoCallServer\mysqlScript.js```는 NodeJS 서버가

DataBase에 접근할 수 있도록 도와주는 스크립트이다.

해당 Script를 열고 다음과 같이 수정한다.

```javascript
var mysql      = require('mysql')
let queryString = require('querystring')
var connection = mysql.createConnection({
  host     : 'localhost',
  user     : '(DB User ID를 적으세요)',
  password : '(DB Password를 적으세요)',
  port     : 3306,
  database : 'holocalldb'
});

// ... 후략 ...
```



###  2.6  Node JS 웹 서버 실행

cmd를 켜고,  ```cd WorkManagerFaceCall\HoloLensVideoCallServe```를 입력한다.

그 후, ```node webServer.js``` 명령어로 서버를 실행한다. 이제 ```http://localhost:8000/```로 접속하면

아래와 같은 테스트 페이지를 볼 수 있다.

![image](https://user-images.githubusercontent.com/40852277/74333064-85219000-4dda-11ea-86f2-e2bf1386a6bb.png)

### 2.7 Unity 2018.1.0f 다운로드

[Unity 다운로드 아카이브](https://unity3d.com/get-unity/download/archive)에서 2018.1.0f를 다운받는다. 

```Unity Hub```을 다운받고 그 후 버전을 다운받는 것을 추천.

다운받아야 하는 모듈은

- Documentation
- Standard Assets
- Example Project
- Windows Store .NET Scripting Backend
- Windows Store IL2CPP Scripting Backend
- Windows IL2CPP Scriping Backend

이다.



---

## 3 . How to use

### 3.1 페이지 구성

페이지는 총 6개로 다음과 같다.

|                   페이지                    |                             설명                             |
| :-----------------------------------------: | :----------------------------------------------------------: |
|        ```http://localhost:8000/```         |        인트로페이지. 사용자가 실제로 보고 있는 페이지        |
|   ```http://localhost:8000/Calling.jsp```   | 전화를 걸 때 접속하는 페이지. <br>주소 뒤에 본인의 아이피 주소를입력해야한다. |
|  ```http://localhost:8000/CallState.jsp```  |              현재 전화의 상태를 확인하는 페이지              |
| ```http://localhost:8000/Connecting.jsp```  |            걸려온 전화를 받을 때 접속하는 페이지             |
|   ```http://localhost:8000/HangUp.jsp```    |                전화를 끊을 때 접속하는 페이지                |
| ```http://localhost:8000/callingPage.jsp``` |                  실제로 통화를 하는 페이지                   |



### 3.2 Unity를 실행

```Unity Hub```를 실행하고 ```WorkManagerFaceCall\VoiceChat```폴더를 프로젝트로 지정하고 연다.

그 후, 플레이 버튼을 누른다.

해당 프로젝트는 데몬 프로그램처럼 항상 켜져 있어야 하므로, 플레이 버튼을 누른 뒤

참을 잠시 내려 놓도록 한다.

(추후 수정 예정)



### 3.3 전화 걸기

추가적인 웹 브라우저 창을 열고,

```http://localhost:8000/Calling.jsp?ip=(자신의아이피)```에 접속한다.

그러면 인트로페이지에서 전화가 왔음을 볼 수 있다.

![image](https://user-images.githubusercontent.com/40852277/74333121-9ec2d780-4dda-11ea-9109-2b8c95bf242b.png)

### 3.4 전화 받기

인트로 페이지에서 전화 받기 버튼을 누르면 전화를 받을 수 있다. 또는  

```http://localhost:8000/Connecting.jsp```에 접속해도 된다.



### 3.5 통화 끊기

전화가 걸려 왔다면 끊기 버튼을 누르던가 또는 

```http://localhost:8000/HangUp.jsp```를 접속하면 된다.



---













