using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuX.Core.db
{
    /// <summary>
    /// 数据库数据
    /// </summary>
    public class DBData
    {
        /// <summary>
        /// 自动创建 <br/> 如果存在此数据库则使用，没有则创建
        /// </summary>
        [Description("自动创建")]
        public bool AutoCreate { get; set; } = true;

        /// <summary>
        /// 数据库文件名称
        /// </summary>
        [Description("数据库文件名称")]
        public string DBFileName { get; set; } = "Com_db_Data.db";

        /// <summary>
        /// 数据库目录（持久化，避免 bin 清理）
        /// </summary>
        [Description("数据库文件路径(目录)")]
        public string DBFilePath { get; set; } =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "FuX", "db");

        /// <summary>
        /// 数据库完整路径
        /// </summary>
        public string DBFullPath => Path.Combine(DBFilePath, DBFileName);

        /// <summary>
        /// 初始化：创建目录 + 从旧位置迁移（可选但强烈建议）
        /// </summary>
        public void EnsureDbExistsAndMigrateIfNeeded()
        {
            Directory.CreateDirectory(DBFilePath);

            if (File.Exists(DBFullPath)) return;

            // 旧位置：以前放在程序目录/bin 下的 db
            var oldPath = Path.Combine(AppContext.BaseDirectory, "db", DBFileName);

            if (File.Exists(oldPath))
            {
                File.Move(oldPath, DBFullPath);
                return;
            }

            if (AutoCreate)
            {
                // SQLite：如果文件不存在，连接时会自动创建空库
                // 这里也可以选择直接创建空文件（非必须）
                // File.Create(DBFullPath).Dispose();
            }
            else
            {
                throw new FileNotFoundException("业务数据库不存在，且 AutoCreate=false", DBFullPath);
            }
        }
    }
}
