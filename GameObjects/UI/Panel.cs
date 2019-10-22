using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace OZ.MonoGame.GameObjects.UI
{
    public class Panel : Control, ICollection<Control>, IEnumerable<Control>
    {
        public int SpaceBetweenControls { get; set; } = 10;

        #region ICollection Implementation
        public int Count => ((ICollection<Control>)Controls).Count;

        public bool IsReadOnly => ((ICollection<Control>)Controls).IsReadOnly;

        bool _toMiddleXAfterLayoutResume = false;
        public void InnerToMiddleX()
        {
            if (GameParent.IsLayoutSuspended)
            {
                _toMiddleXAfterLayoutResume = true;
            }
            else
            {
                float y = RectangleOfContentDrawingMultiplyScale.Y;

                foreach (var control in this)
                {
                    control.Location = new Vector2()
                    {
                        Y = y,
                        X = RectangleOfContentDrawingMultiplyScale.X + (RectangleOfContentDrawingMultiplyScale.Width - control.Size.X) / 2
                    };

                    y += SpaceBetweenControls + control.Size.Y;
                }
            }
        }

        public void Add(Control item)
        {
            ((ICollection<Control>)Controls).Add(item);
            item.Parent = this;
        }

        public void AddRange(params Control[] controls)
        {
            foreach (var control in controls)
            {
                Add(control);
            }
        }

        public void Clear()
        {
            ((ICollection<Control>)Controls).Clear();
        }

        public bool Contains(Control item)
        {
            return ((ICollection<Control>)Controls).Contains(item);
        }

        public void CopyTo(Control[] array, int arrayIndex)
        {
            ((ICollection<Control>)Controls).CopyTo(array, arrayIndex);
        }

        public bool Remove(Control item)
        {
            item.Parent = null;
            return ((ICollection<Control>)Controls).Remove(item);
        }

        public IEnumerator<Control> GetEnumerator()
        {
            return ((ICollection<Control>)Controls).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((ICollection<Control>)Controls).GetEnumerator();
        }
        #endregion ICollection Implementation

        public Panel(GamePrototype gameParent) : base(gameParent)
        {
            LookingChanged += (sender, e) =>
            {
                if (_toMiddleXAfterLayoutResume)
                {
                    InnerToMiddleX();
                    _toMiddleXAfterLayoutResume = false;
                }
            };
        }


        #region Occur Events Methods
        #endregion Occur Events Methods
    }
}
