## 큰사위키
큰사위키는 충남삼성고등학교 학생들이 함께 만들어나가는 학교 위키입니다.
학교 생활, 동아리, 행사, 학습 자료 등 학교와 관련된 모든 정보를 학생들이 직접 기록하고 공유할 수 있어요.

- 모두가 참여할 수 있어요: 학생 누구나 글을 작성하고 편집할 수 있습니다.

- 함께 성장하는 지식: 시간이 지나면서 더 풍부하고 정확한 정보로 발전합니다.

- 우리만의 기록: 학교 생활의 소중한 순간과 이야기를 친구들과 함께 나눌 수 있습니다.

큰사위키는 단순한 정보 저장소가 아니라, 학생들이 스스로 만들어가는 학교 문화의 기록입니다.
여러분도 큰사위키에 참여해서 우리의 이야기를 함께 만들어가요!

## 프로젝트의 목적
학생들이 서로 학교 생활에 대한 이야기를 나누고 동아리, 학교 행사, 정보를 공유할 수 있는 사이트 같은게 있으면 좋을 것 같다고 생각해 이 프로젝트를 진행하게 되었다.

## 주요 기능
- 로그인/회원가입
- 위키 문서 생성, 조회, 수정, 삭제 기능
- 마크다운 기반 문서 작성 및 실시간 미리보기 지원
- 에디터에서 이미지 업로드 및 삽입 지원
- 최근 수정된 문서 목록 제공
- 전체 텍스트 검색 기능 제공(제목 + 본문)
- 급식 메뉴 제공

## 기술 스택
<table>
	<tr>
		<td>프론트엔드</td>
		<td>
			<img src="https://img.shields.io/badge/ASP.NET%20Blazor-512BD4?style=for-the-badge&logo=blazor&logoColor=white" />
		</td>
	</tr>
	<tr>
		<td>백엔드</td>
		<td>
			<img src="https://img.shields.io/badge/ASP.NET-512BD4?style=for-the-badge&logo=.net&logoColor=white" />
		</td>
	</tr>
	<tr>
		<td>개발 언어</td>
		<td>
			<img src="https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=csharp&logoColor=white" />
			<img src="https://img.shields.io/badge/HTML-E34F26?style=for-the-badge&logo=html5&logoColor=white" />
			<img src="https://img.shields.io/badge/CSS-1572B6?style=for-the-badge&logo=css&logoColor=white" />
		</td>
	</tr>
	<tr>
		<td>ORM</td>
		<td>
			<img src="https://img.shields.io/badge/Entity%20Framework%20Core-512BD4?style=for-the-badge&logo=.net&logoColor=white" />
		</td>
	</tr>
	<tr>
		<td>데이터베이스</td>
		<td>
			<img src="https://img.shields.io/badge/Azure%20SQL%20Database-0078D4?style=for-the-badge&logo=microsoft-azure&logoColor=white" />
		</td>
	</tr>
	<tr>
		<td>개발 툴</td>
		<td>
			<img src="https://img.shields.io/badge/Visual%20Studio%202022-5C2D91?style=for-the-badge&logo=visual-studio&logoColor=white" />
			<img src="https://img.shields.io/badge/GitHub-181717?style=for-the-badge&logo=github&logoColor=white" />
		</td>
	</tr>
</table>

## 시스템 구조
![이미지]()

## 폴더 구조
<table>
	<tr>
		<th>디렉토리</th>
		<th>설명</th>
	</tr>
	<tr>
		<td>/</td>
		<td>비주얼 스튜디오 솔루션<td>
	</tr>
	<tr>
		<td>/CNSAWiki</td>
		<td>프론트엔드 프로젝트(UI, Blazor Server App)</td>
	</tr>
	<tr>
		<td>/WikiApi</td>
		<td>백엔드 프로젝트(ASP.NET Core Web Api)</td>
	</tr>
	<tr>
		<td>/SharedData</td>
		<td>두 프로젝트(CNSAWiki, WikiApi)에서 공통으로 사용하는 클래스 정리용 프로젝트(Class Library)</td>
	</tr>
</table>

## 설치 방법
### 1. 프로젝트 클론
```bash
git clone https://github.com/htjung09/CNSA-Wiki
cd CNSA-Wiki
```

### 2. 사전 요구 사항
이 프로젝트를 실행하기 위해선 다음이 필요합니다:
- .NET SDK 8.0 이상
- Visual Studio 또는 Visual Studio Code
- Git
- SQL Server

### 3. 데이터베이스 설정
#### (1) appsettings.json 수정
WikiApi 프로젝트의 appsettings.json 파일에서 ConnectionStrings:DefaultConnection 값을 본인 환경에 맞게 수정

예시)
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=CNSAWiki;Trusted_Connection=True;MultipleActiveResultSets=true"
}
```

#### (2) 데이터베이스 마이그레이션 적용
```bash
cd WikiApi
dotnet ef database update
```

### 4. 백엔드(API 서버) 실행
```bash
cd WikiApi
dotnet run
```

### 5. 프론트엔드와 백엔드 연결하기
CNSAWiki 프로젝트의 Program.cs 파일에서 HttpClient의 BaseAdress를 터미널에 나오는 자신의 API의 주소로 설정

예시)
```cs
builder.Services.AddSingleton(sp => new HttpClient
{
    BaseAddress = new Uri("http://localhost:5000")
});
```

### 6. 프론트엔드(Blazor) 실행
새 터미널을 열고 다음 명령어 실행
```bash
cd CNSA-Wiki/CNSAWiki
dotnet run
```

### 7. 실행 확인
- 터미널에 나오는 주소로 브라우저에서 접속
- 로그인, 문서 작성, 이미지 업로드 등 기능이 정상 작동하는지 확인

## 개발자 소개
<table>
  <tr>
	<th>11130 정형태</th>
	<th>10101 강민재</th>
	<th>10928 채희서</th>
  </tr>
	
  <tr>
    <td>
		<a href="https://github.com/htjung09" title="htjung09 on GitHub">
			<img src="https://github.com/htjung09.png" width="100" height="100" />
		</a>
	</td>
    <td>
		<a href="https://github.com/min268" title="강민재 on GitHub">
			<img src="https://github.com/min268.png" width="100" height="100" />
		</a>
	</td>
    <td>
		<a href="https://github.com/CHS113" title="CHS113 on GitHub">
			<img src="https://github.com/CHS113.png" width="100" height="100" />
		</a>
	</td>
  </tr>
</table>
