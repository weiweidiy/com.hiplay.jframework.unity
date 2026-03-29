using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace JFramework
{
    /// <summary>
    /// 本地文件删除器
    /// </summary>
    public class LocalDeleter : IDelete, IDisposable
    {
        private readonly string _baseDirectory;
        private bool _disposed;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="baseDirectory">基础目录路径</param>
        public LocalDeleter(string baseDirectory = null)
        {
            _baseDirectory = string.IsNullOrWhiteSpace(baseDirectory)
                ? Path.Combine(Environment.CurrentDirectory, "DataStore")
                : baseDirectory;

            Directory.CreateDirectory(_baseDirectory);
        }

        /// <summary>
        /// 获取完整文件路径
        /// </summary>
        private string GetFullPath(string location)
        {
            return Path.Combine(_baseDirectory, location);
        }

        /// <summary>
        /// 删除单个文件
        /// </summary>
        public void Delete(string location)
        {
            var fullPath = GetFullPath(location);
            if (File.Exists(fullPath))
                File.Delete(fullPath);
        }

        /// <summary>
        /// 删除多个文件
        /// </summary>
        public void DeleteAllFiles(string srcPath)
        {
            var fullPath = GetFullPath(srcPath);
            if (!Directory.Exists(fullPath)) return;

            foreach (var file in Directory.GetFiles(fullPath))
            {
                File.SetAttributes(file, FileAttributes.Normal);
                File.Delete(file);
            }
        }

        /// <summary>
        /// 删除目录
        /// </summary>
        public void DelectDir(string srcPath, bool recursive = false)
        {
            var fullPath = GetFullPath(srcPath);
            if (Directory.Exists(fullPath))
                Directory.Delete(fullPath, recursive);
        }

        /// <summary>
        /// 检查文件是否存在
        /// </summary>
        public bool ExistsFile(string filePath)
        {
            return File.Exists(GetFullPath(filePath));
        }

        /// <summary>
        /// 检查目录是否存在
        /// </summary>
        public bool ExistsDirectory(string srcPath)
        {
            return Directory.Exists(GetFullPath(srcPath));
        }

        /// <summary>
        /// 异步删除文件
        /// </summary>
        public async Task<bool> DeleteAsync(string location)
        {
            var fullPath = GetFullPath(location);
            if (!File.Exists(fullPath)) return false;

            await Task.Run(() => File.Delete(fullPath));
            return true;
        }

        /// <summary>
        /// 清空所有存储数据
        /// </summary>
        public async Task ClearAsync()
        {
            await Task.Run(() =>
            {
                if (!Directory.Exists(_baseDirectory)) return;

                // 删除所有文件
                foreach (var file in Directory.GetFiles(_baseDirectory))
                {
                    try
                    {
                        File.SetAttributes(file, FileAttributes.Normal);
                        File.Delete(file);
                    }
                    catch { /* 忽略删除错误 */ }
                }

                // 删除所有子目录
                foreach (var dir in Directory.GetDirectories(_baseDirectory))
                {
                    try
                    {
                        Directory.Delete(dir, true);
                    }
                    catch { /* 忽略删除错误 */ }
                }
            });
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                // 可以在这里添加需要释放的托管资源
            }

            _disposed = true;
        }

        ~LocalDeleter()
        {
            Dispose(false);
        }
    }
}