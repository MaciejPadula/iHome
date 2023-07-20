<script lang="ts">
  import type { WidgetModel } from "../models/widget";
  import { WidgetType } from "../models/widget-type";

  export let widget: WidgetModel;

  function resolveWidgetStyleByType(widgetType: WidgetType): string{
    switch(widgetType) {
      case WidgetType.Small:
        return "small-widget";
      case WidgetType.Medium:
        return "medium-widget";
      case WidgetType.Wide:
        return "wide-widget";
      default:
        return "";
    }
  };

  function widgetStyle(): string {
    const additionalStyle = widget.showBorder ? ' widget-border' : '';
    return resolveWidgetStyleByType(widget.widgetType) + additionalStyle;
  }
</script>

<div class={`${widgetStyle()} widget`}>
  <slot></slot>
</div>

<style>
.widget {
  height: 10rem;
  box-sizing: border-box;
  display: flex;
  flex-direction: row;
  align-items: center;
  justify-content: center;
}

.widget-border{
  border: solid #999999 1px;
}

.small-widget {
  width: 48vw;
}

.medium-widget {
  width: 96vw;
}

.wide-widget {
  width: 96vw;
}

@media (min-width: 640px) {
  .small-widget {
    width: 24vw;
  }
    
  .medium-widget {
    width: 48vw;
  }
    
  .wide-widget {
    width: 96vw;
  }
}
</style>