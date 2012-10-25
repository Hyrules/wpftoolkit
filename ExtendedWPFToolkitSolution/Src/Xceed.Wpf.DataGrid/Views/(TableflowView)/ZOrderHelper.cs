﻿/************************************************************************

   Extended WPF Toolkit

   Copyright (C) 2010-2012 Xceed Software Inc.

   This program is provided to you under the terms of the Microsoft Public
   License (Ms-PL) as published at http://wpftoolkit.codeplex.com/license 

   This program can be provided to you by Xceed Software Inc. under a
   proprietary commercial license agreement for use in non-Open Source
   projects. The commercial version of Extended WPF Toolkit also includes
   priority technical support, commercial updates, and many additional 
   useful WPF controls if you license Xceed Business Suite for WPF.

   Visit http://xceed.com and follow @datagrid on Twitter.

  **********************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Windows;
using System.Windows.Controls;

namespace Xceed.Wpf.DataGrid.Views
{
  internal class ZOrderHelper
  {
    /// <param name="children">The Children of the Panel</param>
    /// <param name="zIndexMapping">The int[] of ZOrder mapping</param>
    /// <returns>True if a mapping was performed, false if nothing changed</returns>
    public static bool ComputeZOrder( IList<UIElement> children, ref int[] zIndexMapping )
    {
      if( children == null )
      {
        return false;
      }

      int count = children.Count;
      List<long> tempList = new List<long>( count );

      bool needMapping = false;

      if( count > 0 )
      {
        UIElement element = children[ 0 ] as UIElement;

        int zIndex = ( element != null )
          ? Panel.GetZIndex( element )
          : 0;

        tempList.Add( ( long )zIndex << 32 );

        if( count > 1 )
        {
          int currentItemIndex = 1;
          int lastZIndex = zIndex;

          do
          {
            element = children[ currentItemIndex ] as UIElement;

            zIndex = ( element != null )
              ? Panel.GetZIndex( element )
              : 0;

            tempList.Add( ( ( long )zIndex << 32 ) + currentItemIndex );

            needMapping |= ( zIndex < lastZIndex );
            lastZIndex = zIndex;
          }
          while( ++currentItemIndex < count );
        }
      }


      if( needMapping )
      {
        tempList.Sort();

        if( ( zIndexMapping == null ) || ( zIndexMapping.Length != count ) )
        {
          zIndexMapping = new int[ children.Count ];
        }

        for( int i = 0; i < count; i++ )
        {
          zIndexMapping[ i ] = ( int )( tempList[ i ] & 0xFFFFFFFFL );
        }
      }
      else
      {
        zIndexMapping = null;
      }

      return needMapping;
    }
  }
}