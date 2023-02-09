import { WidgetType } from "./widget-type";

export interface Widget {
    id: string,
    widgetType: WidgetType,
    roomId: string,
    showBorder: boolean
}
