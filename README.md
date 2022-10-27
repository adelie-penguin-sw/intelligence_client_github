# Intelligence_Client

SW마에스트로의 경우 팀빌딩이 되어가고 있는 팀은 대부분 앱과 웹서비스를 목표한 팀이었습니다. 이에 많은 고민을 하던 중 소프트웨어연수생 메신저에 게임 프로젝트를 진행하고 싶은 마음을 담아 팀빌딩 관련 글을 올리게 되었습니다. 운 좋게도 마음에 맞는 팀원들을 만나 게임 개발 프로젝트를 진행할 수 있었습니다. 11월 30일까지 진행하는 프로젝트로 배포를 목표로 하여 제작하고 있습니다. 현재 소마에서 제공하는 private GitLab을 사용하여 형상관리를 진행하고 있습니다.
<br>
<h2>프로젝트 소개</h2>

**장르 :** 2D 방치형 게임

**담당 업무 :** 클라이언트 개발

**사용 기술 :** Unity, C#

**간단한 게임 소개**
<br> 
과학자(유저)는 초지능을 극도로 발달시켜 우주 전체를 시뮬레이션하는 실험을 하고자 한다.
우주를 100% 시뮬레이션하려면 특정 수준 이상의 지능이 필요하다.
그러나 하나의 브레인만 가지고는 이 지능 수준에 절대 도달할 수 없다.
따라서 다수의 브레인들을 연결한 네트워크인 "초지성체"를 구축하여 이 문제를 해결한다.

<br>
<h2>Getting started</h2>
install Unity 2020.3.40f1

<br>
<h2>ClientSide Stack</h2>

**Client**<br>
<img src="https://img.shields.io/badge/C%20Sharp-239120?style=for-the-badge&logo=C%20Sharp&logoColor=white"> <img src="https://img.shields.io/badge/Unity-222324?style=for-the-badge&logo=Unity&logoColor=white">

**Other**<br>
<img src="https://img.shields.io/badge/Jira%20Software-0052CC?style=for-the-badge&logo=Jira%20Software&logoColor=white"> <img src="https://img.shields.io/badge/Confluence-172B4D?style=for-the-badge&logo=Confluence&logoColor=white"> <img src="https://img.shields.io/badge/Git-F05032?style=for-the-badge&logo=Git&logoColor=white"> <img src="https://img.shields.io/badge/GitLab-FC6D26?style=for-the-badge&logo=GitLab&logoColor=white">

<br>
<h2>Develop</h2>

**Code Convention**<br>
참고 - https://learn.microsoft.com/ko-kr/dotnet/csharp/fundamentals/coding-style/coding-conventions<br>
주석 - https://chaesoo.tistory.com/m/172<br>

**Custom Rules**<br>
**Base**<br>
유니티의 생명주기를 가지고 있지 않는다.<br>
싱글톤 X<br>
직접적으로 쓰이는 스크립트가 아닌 다른 스크립트의 부모가 되는 스크립트.<br>
ex) PopupBase(부모) → InventoryPopup(자식) <br>

**AMVCC**<br>
https://chaesoo.tistory.com/m/168

**Manager**<br>
유니티의 생명주기를 가지고 있음.<br>
싱글톤으로 구현 될 수 있음.<br>

<br>
<h2>ClassDiagram</h2>

![image](https://user-images.githubusercontent.com/26276038/196415670-254a0ef5-e2cd-428e-938a-0badc6ed716d.png)

<br>
<h2>사용 라이브러리 및 플러그인</h2>

**AppleLogin** https://github.com/lupidan/apple-signin-unity#installation<br>
version: v1.4.2<br>
사용목적: 애플로그인 구현<br>
사용결과: 애플로그인 구현완료<br>

**GooglePlayLogin** https://github.com/playgameservices/play-games-plugin-for-unity<br>
version: v10.14<br>
사용목적: 구글플레이로그인 구현<br>
사용결과: 구글플레이로그인 구현완료<br>

**UniTask** https://github.com/Cysharp/UniTask#getting-started<br>
version: v2.3.1<br>
사용목적: 네트워크 통신시 비동기 구조 변경<br>
사용결과: 기존 사용하던 함수들의 action callback지옥에서 벗어나 return으로 해결<br>

<br>
<h2>결과물 형태</h2>

**.apk .ipa**<br>
유니티의 장점인 다중 플랫폼 빌드를 살려 Android, iOS 모두 제공하는 방향으로 개발하고 있습니다.<br>
현재 구글플레이, 애플로그인을 제공하고있습니다.
