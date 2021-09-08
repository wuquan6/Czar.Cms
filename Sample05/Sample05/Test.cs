using Dapper;
using Sample05.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Sample05
{
    class Test
    {
        /// <summary>
        /// 测试插入一条数据
        /// </summary>
        public static void test_insert()
        {
            var content = new Content
            {
                title = "标题1",
                content = "内容1"
            };
            using(var conn = new SqlConnection("Data Source=.;User ID=sa;Password=123456;Initial Catalog=coreContent;Pooling=true;Max Pool Size=100;"))
            {
                string sql_insert = @"INSERT INTO [Content]
                (title, [content], status, add_time, modify_time)
VALUES   (@title,@content,@status,@add_time,@modify_time)";
                var result = conn.Execute(sql_insert, content);
                Console.WriteLine($"test_insert: 插入了{result}条数据！");
            }
        }
        /// <summary>
        /// 测试插入两条数据
        /// </summary>
        public static void test_mult_insert()
        {
            List<Content> contents = new List<Content>()
            {
                new Content
                {
                    title = "批量插入标题1",
                    content="批量插入内容1"
                },
                new Content
                {
                     title = "批量插入标题2",
                    content="批量插入内容2"
                }
            };

            using (var conn = new SqlConnection("Data Source=.;User ID=sa;Password=123456;Initial Catalog=coreContent;Pooling=true;Max Pool Size=100;"))
            {
                string sql_insert = @"INSERT INTO [Content]
                (title, [content], status, add_time, modify_time)
VALUES   (@title,@content,@status,@add_time,@modify_time)";
                var result = conn.Execute(sql_insert, contents);
                Console.WriteLine($"test_insert: 插入了{result}条数据！");
            }
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public static void test_del()
        {
            var content = new Content
            {
                id = 2
            };
            using(var conn = new SqlConnection("Data Source=.;User ID=sa;Password=123456;Initial Catalog=coreContent;Pooling=true;Max Pool Size=100;"))
            {
                string sql_del = @"DELETE FROM [Content] where (id = @id)";
                var result = conn.Execute(sql_del, content);
                Console.WriteLine($"test_del: 删除了{result}条记录");
            }
        }
        /// <summary>
        /// 删除多条数据
        /// </summary>
        public static void test_mult_del()
        {
            List<Content> contents = new List<Content>()
            {
                new Content
                {
                    id=3,
                },
                new Content
                {
                    id = 1,
                }
            };

            using (var conn = new SqlConnection("Data Source=.;User ID=sa;Password=123456;Initial Catalog=coreContent;Pooling=true;Max Pool Size=100;"))
            {
                string sql_del = @"DELETE FROM [Content] where (id = @id)";
                var result = conn.Execute(sql_del, contents);
                Console.WriteLine($"test_del: 删除了{result}条记录");
            }
        }
        /// <summary>
        /// 修改一条数据
        /// </summary>
        public static void test_update()
        {
            var content = new Content
            {
                id = 2,
                title = "标题666",
                content = "内容666"
            };

            using(var conn = new SqlConnection("Data Source=.;User ID=sa;Password=123456;Initial Catalog=coreContent;Pooling=true;Max Pool Size=100;"))
            {
                string sql_update = @"update [Content] SET  title = @title, [content] = @content, modify_time = GETDATE() WHERE   (id = @id)";
                int result = conn.Execute(sql_update, content);
                Console.WriteLine($"sql_update{result}条数据");
            }
        }

        public static void test_mult_update()
        {
            List<Content> contents = new List<Content>()
            {
                new Content
                {
                    id = 4,
                title = "标题aaa",
                content = "内容aaa"
                },
                new Content
                {
                    id = 5,
                title = "标题bbb",
                content = "内容bbb"
                }
            };

            using (var conn = new SqlConnection("Data Source=.;User ID=sa;Password=123456;Initial Catalog=coreContent;Pooling=true;Max Pool Size=100;"))
            {
                string sql_update = @"update [Content] SET  title = @title, [content] = @content, modify_time = GETDATE() WHERE   (id = @id)";
                int result = conn.Execute(sql_update, contents);
                Console.WriteLine($"sql_update{result}条数据");
            }
        }

        public static void test_select_one()
        {
            using (var conn = new SqlConnection("Data Source=.;User ID=sa;Password=123456;Initial Catalog=coreContent;Pooling=true;Max Pool Size=100;"))
            {
                string sql_insert = @"select * from [dbo].[content] where id=@id";
                var result = conn.QueryFirstOrDefault<Content>(sql_insert, new { id = 5 });
                Console.WriteLine($"test_select_one：查到的数据为：{result.content}");
            }
        }

        public static void test_select_content_with_comment()
        {
            using (var conn = new SqlConnection("Data Source=.;User ID=sa;Password=123456;Initial Catalog=coreContent;Pooling=true;Max Pool Size=100;"))
            {
                string sql_insert = @"select * from content where id=@id;
select * from comment where content_id=@id;";
                using (var result = conn.QueryMultiple(sql_insert, new { id = 5 }))
                {
                    var content = result.ReadFirstOrDefault<ContentWithComment>();
                    content.comments = result.Read<Comment>();
                    Console.WriteLine($"test_select_content_with_comment:内容5的评论数量{content.comments.Count()}");
                }

            }
        }
    }
    
}
