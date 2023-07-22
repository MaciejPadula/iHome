<script lang="ts">
  import { isAuthenticated, loading } from '../../../data-access/store';
  import { getRoomWidgets } from '../../../data-access/api/widgets-service.js';
  import Widget from '../../../components/Widget.svelte';
  import WaitingComponent from '../../../components/WaitingComponent.svelte';
  import { widgets } from '../rooms-store';
  import GuardComponent from '../../../components/GuardComponent.svelte';
  
  export let data;
  
  isAuthenticated.subscribe(async authed => {
    if(!authed) return;

    await loadWidgets();
  });

  async function loadWidgets() {
    loading.set(true);
    var result = await getRoomWidgets(data.id);
    widgets.set(result);
    loading.set(false);
  }
</script>

<GuardComponent>
  <div class="w-full flex flex-row flex-wrap items-center justify-center">
    <WaitingComponent>
      {#each $widgets as widget}
        <Widget bind:widget={widget}>
          <div>{widget.id}</div>
        </Widget>
      {/each}
    </WaitingComponent>
  </div>
</GuardComponent>