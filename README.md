# SnakeGame
## 2. Unity2D (2022.01.17 ~ )

## 1. Cocos Creator (2022.01.10 ~ 2022.01.14)
### 개발 환경
- windows 10
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
