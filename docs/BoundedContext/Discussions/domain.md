# Domain Model (v1)
Discussion context:
| Model      | Type           |
| ---------- | -------------- |
| Discussion | Aggregate Root |
| Comment    | Entity         |


# Discussion - Aggregate Root
| Field        | Purpose                      |
| ------------ | ---------------------------- |
| Id           | Unique identifier            |
| ProblemId    | Discussion thuộc problem nào |
| AuthorId     | Người tạo discussion         |
| Title        | Tiêu đề thread               |
| Content      | Nội dung chính               |
| CreatedAt    | Thời gian tạo                |
| UpdatedAt    | Lần sửa cuối                 |
| IsLocked     | Admin lock thread            |
| CommentCount | Cache số comment             |

# Comment - Entities
---------
Id (CommentId)
DiscussionId
AuthorId
Content
CreatedAt
UpdatedAt
ParentCommentId (optional)


