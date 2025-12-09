# StatSystem

캐릭터가 사용할 변수들을 스텟 단위로 묶어서 대신 관리해주는 패키지

## 뭘 할수있지?

- StatDefinition으로 스텟 정의 및 StatDatabase에 스텟 타입 등록
- 다른 스텟의 값을 참조하여 계산된 값을 가지는 `IDerived` 스텟 기능
- Modifier 추가를 통해 Flat, Percent, Multipler 기반 계산 구조 지원
- `IEffectApplicable` 스텟의 CurrentValue 를 변경하는 Effect 기능

## 사용방법

1. 우클릭 → Create → DaveAssets → Stat Database 만들기
2. StatDefinition으로 각 필요한 스텟을 정의하고 DB에 등록
3. 캐릭터에 StatController 컴포넌트 붙이고 → 만든 데이터베이스 연결
4. 상호작용을 통해서 StatController의 StatHandler에 접근하거나, 상대 StatHandler에 접근
5. Modifier 혹은 Effect를 전달하여 값 적용
