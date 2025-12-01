# StatSystem

StatSystem은 프로젝트에서 사용할 스탯들을 StatDatabase ScriptableObject 안에서 정의·관리할 수 있도록 제공하는 패키지입니다.
스탯을 개별 에셋으로 만들 필요 없이, 데이터베이스 내부에서 직접 추가하고 수정할 수 있습니다.

## 특징
- StatDatabase 중심 관리
단일 ScriptableObject 내부에서 스탯을 생성하고 편집하는 구조.

- NodeGraphEditor 연동
노드 그래프에서 스탯 이름으로 값을 참조하고, 계산된 값을 데이터베이스에 다시 기록 가능.

유연한 시스템 연계
Core 모듈과 연결하여 게임 로직에서 즉시 활용할 수 있음.

## 사용 방법

1. 패키지를 프로젝트에 추가
2. ScriptableObject로 스탯 데이터베이스 생성
3. StatDatabase 내부에서 필요한 스탯을 추가해서 구성
4. 노드그래프를 생성해서 스탯 값 계산을 StatDatabase 등록하여 