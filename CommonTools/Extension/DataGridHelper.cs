using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

//========================================================================
// Copyright(C): ***********************
//
// CLR Version : 4.0.30319.42000
// NameSpace : HZZG.Common.Tolls.Tools
// FileName : DataGridHelper
//
// Created by : XHL at 2020/7/20 20:39:49
//
// Function : 
//
//========================================================================
namespace HZZG.Common.Tolls
{
    public static class DataGridHelper
    {
        /// <summary>
        /// 获取DataGrid的行
        /// </summary>
        /// <param name="dataGrid">DataGrid控件</param>
        /// <param name="rowIndex">DataGrid行号</param>
        /// <returns>指定的行号</returns>
        public static DataGridRow GetRow(this DataGrid dataGrid, int rowIndex)
        {
            DataGridRow rowContainer = (DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromIndex(rowIndex);
            if (rowContainer == null)
            {
                dataGrid.UpdateLayout();
                dataGrid.ScrollIntoView(dataGrid.Items[rowIndex]);
                rowContainer = (DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromIndex(rowIndex);
            }
            return rowContainer;
        }

        /// <summary>
        /// 获取DataGrid控件单元格
        /// </summary>
        /// <param name="dataGrid">DataGrid控件</param>
        /// <param name="rowIndex">单元格所在的行号</param>
        /// <param name="columnIndex">单元格所在的列号</param>
        /// <returns>指定的单元格</returns>
        public static DataGridCell GetCell(this DataGrid dataGrid, int rowIndex, int columnIndex)
        {
            DataGridRow rowContainer = dataGrid.GetRow(rowIndex);
            if (rowContainer != null)
            {
                DataGridCellsPresenter presenter = GetVisualChild<DataGridCellsPresenter>(rowContainer);
                DataGridCell cell = (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(columnIndex);
                if (cell == null)
                {
                    dataGrid.ScrollIntoView(rowContainer, dataGrid.Columns[columnIndex]);
                    cell = (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(columnIndex);
                }
                return cell;
            }
            return null;
        }

        /// <summary>
        /// 获取模板中的控件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataGrid"></param>
        /// <param name="rowIndex"></param>
        /// <param name="columnIndex"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static T FindChildByName<T>(this DataGrid dataGrid, int rowIndex, int columnIndex, string name) where T : Visual
        {
            Visual parent = GetCell(dataGrid, rowIndex, columnIndex);
            if (parent != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
                {
                    var child = VisualTreeHelper.GetChild(parent, i) as Visual;
                    string controlName = child.GetValue(Control.NameProperty) as string;
                    if (controlName == name)
                    {
                        return child as T;
                    }
                    else
                    {
                        T result = FindVisualChildByName<T>(child, name);
                        if (result != null)
                            return result;
                    }
                }
            }
            return null;
        }
        /// <summary>
        /// 获取父控件中的控件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parent">当前的ColumnCell</param>
        /// <param name="name">控件的名称</param>
        /// <returns></returns>
        public static T FindVisualChildByName<T>(Visual parent, string name) where T : Visual
        {
            if (parent != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
                {
                    var child = VisualTreeHelper.GetChild(parent, i) as Visual;
                    string controlName = child.GetValue(Control.NameProperty) as string;
                    if (controlName == name)
                    {
                        return child as T;
                    }
                    else
                    {
                        T result = FindVisualChildByName<T>(child, name);
                        if (result != null)
                            return result;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 获取父可视对象中第一个指定类型的子可视对象
        /// </summary>
        /// <typeparam name="T">可视对象类型</typeparam>
        /// <param name="parent">父可视对象</param>
        /// <returns>第一个指定类型的子可视对象</returns>
        public static T GetVisualChild<T>(Visual parent) where T : Visual
        {
            T childContent = default(T);
            int numVisuals = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < numVisuals; i++)
            {
                Visual v = (Visual)VisualTreeHelper.GetChild(parent, i);
                childContent = v as T;
                if (childContent == null)
                {
                    childContent = GetVisualChild<T>(v);
                }
                if (childContent != null)
                {
                    break;
                }
            }          
            return childContent;
        }
        /// <summary>
        /// 获取默认列的元素，比如DataGridCheckBoxColumn，DataGridTextColumn等现有的列
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataGrid"></param>
        /// <param name="rowIndex"></param>
        /// <param name="columnIndex"></param>
        /// <returns></returns>
        public static T GetDefaultColumnCellContent<T>(this DataGrid dataGrid, int rowIndex,int columnIndex)where T :Visual
        {
            T childContent = default(T);
            DataGridRow row = dataGrid.GetRow(rowIndex);
            if (row != null)
            {
                FrameworkElement fe = dataGrid.Columns[columnIndex].GetCellContent(row);
                if (fe != null)
                {
                    childContent = fe as T;
                    if (childContent == null)
                    {
                        return null;
                    }
                }
            }
            return childContent;
        }
        /// <summary>
        /// 获取列的模板
        /// </summary>
        /// <param name="dataGrid"></param>
        /// <param name="columnIndex"></param>
        /// <returns></returns>
        public static DataGridTemplateColumn GetDataGridTemplate(this DataGrid dataGrid, int columnIndex)
        {
            return dataGrid.Columns[columnIndex] as DataGridTemplateColumn;
        }
    }
}
