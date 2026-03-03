# A. ProblemManagement Creation flow

### UC-01: CreateProblem

### UC-02: UpdateProblem

### UC-03: AddTestCase

### UC-04: RemoveTestCase

### UC-05: ReorderTestCase (optional)

### UC-06: AddAllowedLanguage

### UC-07: RemoveAllowedLanguage

### UC-08: AssignClassificationToProblem

### UC-09: RemoveClassificationFromProblem  

### UC-10: PublishProblem
    
### UC-11: ArchiveProblem

# B. Query Flow (Read Model)

### UC-12: GetProblemDetail

### UC-13: GetPublishProblemDetail

### UC-14: GetProlemList


# NHÓM 1 – LIFECYCLE (BẮT BUỘC)

| Command          | Bắt buộc | Ghi chú        |
| ---------------- | -------- | -------------- |
| `CreateProblem`  | ✅        | Khởi tạo Draft |
| `PublishProblem` | ✅        | Mở submission  |
| `ArchiveProblem` | ✅        | Đóng problem   |

# NHÓM 2 – AUTHORING (DRAFT MODE)

| Command                  | Bắt buộc | Vì sao          |
| ------------------------ | -------- | --------------- |
| `UpdateProblemMetadata`  | ✅        | Edit đề         |
| `UpdateConstraints`      | ✅        | Judge cần       |
| `AddTestCase`            | ✅        | Core            |
| `RemoveTestCase`         | 🟡       | Sửa lỗi test    |
| `ReorderTestCases`       | 🟡       | Optional        |
| `AddAllowedLanguage`     | ✅        | Core            |
| `RemoveAllowedLanguage`  | 🟡       | Hiếm            |
| `AssignClassification`   | ✅        | Search / filter |
| `UnassignClassification` | 🟡       | Cleanup         |

# NHÓM 3 – POST-PUBLISH (HẠN CHẾ)

| Command                 | Có không | Rule                    |
| ----------------------- | -------- | ----------------------- |
| `UpdateProblemMetadata` | ⚠️       | Chỉ title / description |
| `ChangeDifficulty`      | ⚠️       | Metadata only           |
| `HideProblem`           | ❌        | Dùng Archive            |

- Problem List
- Problem Detail
- Problem Editor Detail
- Problem Constraints Snapshot

