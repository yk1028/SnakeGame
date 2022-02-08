# SnakeGame
## 3. Custom Multiplay with Unity(2022.02.04 ~ )
### 개발 환경
- Windows 10
- Unity 2020.3.26f1

### 사용 언어
- C#
- JavaScript

### 테스트 환경
- PC (Windows 10)
- X4 (Android 9)

### 개발 순서
1. [x] Client Networking (C#)
2. [x] Server Networking (node.js)
3. [x] Snake 동기화
    1. [x] 머리 동기화
    2. [x] 꼬리 개수 동기화
 1. [ ] Apple 동기화
    1. [ ] Apple 랜덤한 위치에 생성
    2. [ ] Snake가 Apple 먹었는지 판단 후 재배치
    3. [ ] Apple 먹었으면 Snake 꼬리 증가
 2. [ ] 패배 조건 추가
 3. [ ] 승리 조건 추가
 4. [ ] 동시 종료

## 3. Multiplay with Unity(2022.01.24 ~ 2022.02.03)
### 개발 환경
- Windows 10
- Unity 2020.3.26f1

### 사용 언어
- C#

### 테스트 환경
- PC (Windows 10)
- X4 (Android 9)

### 개발 순서
1. [ ] ~~Mirror~~
   1. [x] 머리 방향 동기화
   2. [x] 꼬리 그리기 동기화
2. [x] PUN2 (Photon Unity Networking)
   1. [x] Snake 구현
      1. [x] 머리 이동 구현 (터치로 이동)
      2. [x] 머리 이동 방향으로 회전
      3. [x] 꼬리 구현
   2. [x] Apple(먹이) 구현
      1. [x] Apple 랜덤한 위치에 생성
      2. [x] Snake가 Apple 먹었는지 판단 후 재배치
      3. [x] Apple 먹었으면 Snake 꼬리 증가
   3. [x] 패배 조건 추가
   4. [x] 승리 조건 추가
   5. [x] 동시 종료

### 고민했던 점
- **동기화**
  - 초기 생각 : third party package를 사용하면 잘 될 것이라고 막연하게 생각했다.
  - 문제점 : Mirror를 활용해서 개발했을때 동기화가 너무 맞지 않았다. 내부 동작 원리를 몰랐기 때문에 단기간내에 해결할 수 없었다.
  - 해결 : Photon을 활용해서 좀 더 동기화가 잘 되도록 구현했다. 하지만 완벽하게 싱크가 맞지는 않았다. MultiPlay에 대한 좀 더 자세한 이해가 필요한 것 같다. 다음 미션을 진행하면서 동기화에 대해서 더 깊게 알아봐야겠다.
- **third party package 선택**
  - 초기 생각 : https://blog.unity.com/technology/choosing-the-right-netcode-for-your-game 이 글만 보고 Mirror가 적합하다고 생각해서 선택했다.
  - 문제점 : 자료가 비교적 많지 않아 개발하는데 막히는 부분을 해결하기 쉽지 않았다. 직접 코드를 확인해봐야 했기 때문에 시간이 오래 걸렸다.
  - 해결 : 성능이나 가격에 대한 부분도 중요하지만 개발 기간과 자료의 양도 고려해서 좀 더 신중한 선택이 필요해 보인다.

### 다음 개발에서 생각해볼 부분
- QuickStart나 tutorial을 진행할때 단순히 기능의 동작만 확인하지 말고 원리를 생각하자
- 구글링전에 docs부터 확인
- 개발 진행 상황을 좀 더 구체적으로 공유
- Server와 Client를 어떤 방식으로 구성할 것인가?
- Position과 Rotation의 동기화를 어떻게 구현할 것인가?
- 통신 방식은 어떤 것을 사용할 것인가?
- RPC, RaiseEvent 등을 어떨게 구현할 것인가?

## 2. Unity2D (2022.01.17 ~ 2022.01.21)
### 개발 환경
- Windows 10
- Unity 2020.3.26f1

### 사용 언어
- C#

### 테스트 환경
- PC (Windows 10)
- X4 (Android 9)
- iPhone 13 (iOS 15.0)

### 개발 순서
1. [x] Snake 구현
   1. [x] 머리 이동 구현 (터치로 이동)
   2. [x] 머리 이동 방향으로 회전
   3. [x] 꼬리 구현
2. [x] Apple(먹이) 구현
   1. [X] Apple 랜덤한 위치에 생성
   2. [x] Snake가 Apple 먹었는지 판단 후 재배치
   3. [x] Apple 먹었으면 Snake 꼬리 증가
3. [x] 배경 그리기
4. [x] 종료 조건 판단
   1. [x] 벽에 부딪힌 경우
5. [x] 시작 메뉴 및 재시작 기능 구현

6. [x] Snake 머리 방향 좌우에 맞게 조정
7. [x] GameObject 재사용 가능하게 수정

### 고민했던 점
- **GameObject Component 추가**
  - 초기 생각 : GameObject, Component를 editor에서 생성한 것과 script에서 생성한 것에 대한 구분이 모호했다.
  - 문제점 : script에서 GameObject와 Component들을 활용할 때 햇갈리고 중복되는 코드들이 많았다.
  - 해결 : 전체 클래스 구조를 그려보면서 editor에서 생성한 것과 script에서 생성한 것의 구분을 명확히 했다. visualize가 잘 되어있는 editor활용시 editor에서 생성된 객체가 script에서 생성한 객체의 차이를 명확히 해야겠다.
- **Tirgger Callback Method 추가**
  - 초기 생각 : 당연히 같은 Tirgger Callback Method(ex. OnTriggerEnter2D())는 하나만 등록된다고 생각했다.
  - 문제점 : Trigger Callback Method 추가 방식이 OnTriggerEnter2D()가 작성된 AppleTrigger(MonoBehaviour)클래스 자체를 Component로 추가했기 때문에 재시작할때마다 Callback Method가 추가로 등록되었다. 코드의 구조에 문제점이 있긴 했지만 선입견 때문에 문제점을 찾기 어려웠다.
  - 해결 : 한번만 추가되도록 수정했다. 메서드 단위가 아니라 GameObject와 Component의 관계를 잘 생각해야겠다.
- **GameObject 재사용**
  - 초기 생각 : 재시작 할때마다 모든 Object 생성 후 종료시 Destory메서드를 활용하여 제거했다.
  - 문제점 : 간단한 게임이었기 때문에 문제는 없었지만 재사용 가능한 GameOject의 경우 재사용하는것이 좋다고 생각했다.
  - 해결 : GameObject pool을 만들고 Active 상태를 변경하는 방식으로 수정했다.
- **이동하는 이미지**
  - 초기 생각 : Cocos에서 개발할때 Snake의 머리의 상하좌우 구분이 없었기 때문에 머리이미지의 회전이나 반전을 개발전에 고려하지 않았다.
  - 문제점 : 방향 전환에 따른 머리 이미지의 회전, 반전이 필요했다. 구현시에 회전 각도를 0 ~ 360으로 생각했는데 -180 ~ 180이었다.([Quaternion과 Euler angle](https://killu.tistory.com/12))
  - 해결 : 이동 방향에 대한 백터 정보는 이미 있었기 때문에 계산을 통해 각도를 구하고 각도를 바탕으로 이미지를 회전, 반전시켰다.

### 다음 개발에서 생각해볼 부분
- 개발 초기에 간단하게 빌드를 해보고 개발
- 디바이스의 차이 생각해보면서 개발
- Snake와 Apple 재사용 가능하도록 수정
- Unity가 멀티플레이 지원방식이 HLAPI -> Netcode로 바뀐 이유
- git branch 나눠서 작업
- Unity test 방식 찾아보기
- .gitignore 프로젝트마다 적용하기
  
****
## 1. Cocos Creator (2022.01.10 ~ 2022.01.14)
### 개발 환경
- Windows 10
- Cocos creator 3.4
### 사용 언어
- TypeScript
### 테스트 환경
- web-mobile(chrome, iPhone)
### 개발 순서
1. [ ] Snake(사용자) 만들기
   1. [x] 노드 생성
   2. [x] 이동 스크립트 작성 (머리)
   3. [x] 이동 스크립트 작성 (꼬리)
   4. [ ] ~~그래픽 적용~~
2. [x] GameManager 만들기
   1. [x] Apple(먹이) 랜덤한 위치에 생성
   2. [x] Snake가 Apple을 먹었는지 판단 후 Apple 재배치
   3. [x] Apple 먹었으면 Snake 꼬리 증가
   4. [x] 종료 조건 판단하기
      1. [x] 자기 몸에 부딪힌 경우
      2. [x] 벽에 부딪힌 경우 경우
   5. [x] 시작 메뉴 및 재시작 기능
3. [x] 점수 표시하기

4. [x] 이동 방식 변경 (방향키-> 터치)

### 고민했던 점
- **꼬리 그리는 방식**
   - 초기 생각: 머리 위치를 queue형태로 저장하고 저장된 위치 정보로 꼬리 그리기
   - 문제점 : 너무 짧고 일정하지 않은 delta time 때문에 update() 메서드 호출시마다 머리의 위치를 저장 할 수 없었다.
   - 해결 : 일정 시간 단위(머리 크기만큼 이동할 시간)로 머리 위치를 저장하고 그 중간 위치는 deltaTime을 활용해 거리 비율을 맞춰 그렸다.
- **충돌 감지**
  - 초기 생각 : 좌표 일치를 통해 충돌 판단
  - 문제점 : deltaTime이 짧고 소수점이 있는 좌표계를 활용하는 경우 정확한 충돌 판단이 쉽지 않다.
  - 해결 : 임시 방편으로 오브젝트간의 거리 차이를 이용해 대략적으로 충돌을 판단했다. Collider를 활용하면 더 정확한 충돌을 감지 할 수 있을 것 같다.
- **게임 내 오브젝트들의 좌표와 터치 이벤트 좌표의 차이**
   - 초기 생각 : 단순하게 좌표의 차이를 활용해 방향만 찾으면 쉽게 구현할 수 있다고 생각했다.
   - 문제점 : 터치 이벤트의 좌표와 내부 오브젝트들의 좌표의 동작 방식 자체가 달랐다.
   - 해결 : https://discuss.cocos2d-x.org/t/understanding-the-principles-of-the-coordinate-system/54108

### 다음 개발에서 생각해볼 부분
   - 처음부터 게임의 LifeCycle을 생각하기
   - 어떤 디바이스에서 플레이할지 생각하고 조작법 정하기
   - 언어에서 지원하는 자료구조 구현체 사용시 문서 읽어보기
   - 자주 변경이 필요한 객체(ex. Vec3) 사용시 참조 주의
   - Collider를 활용한 충돌 감지
   - 꼬리 그리기 방식 개선
   - 장애물(다른 Snake, 단순 장애물) 등 게임적인 요소 추가
   - 그래픽 개선
