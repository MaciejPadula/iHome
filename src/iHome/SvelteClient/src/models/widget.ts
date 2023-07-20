import type { WidgetType } from "./widget-type";

export interface WidgetModel {
    id: string,
    widgetType: WidgetType,
    roomId: string,
    showBorder: boolean
}
