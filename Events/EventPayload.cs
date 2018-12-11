using System;

/*
 * All events are sent with an event payload, a data type
 * that encodes what it can be decoded to for type checking.
 */
namespace TUFFYCore.Events
{
    public class EventPayload
    {

        public PayloadContentType ContentType
        {
            get { return _contentType; }
        }

        private PayloadContentType _contentType;
        private Object _content;

        public EventPayload(PayloadContentType contentType, Object content)
        {
            _contentType = contentType;
            _content = content;
            //TODO: add checking here to see if contentType is legal cast for content
        }


        public Object GetContent()
        {
            return _content;
        }

        public Object GetContent(PayloadContentType type)
        {
            switch (type)
            {
                case PayloadContentType.String:
                    if (_content is string)
                    {
                        return (String)_content;
                    } else {
                        //Fails
                        return null;
                    }
                default:
                    return _content;
            
            }
        }

    }
}
