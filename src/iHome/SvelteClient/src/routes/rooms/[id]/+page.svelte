<script lang="ts">
  import { ListPlaceholder } from 'flowbite-svelte'
  import { Skeleton } from 'flowbite-svelte'
  import { onMount } from 'svelte';
  import { loading, widgets } from '../../../data-access/store';
  import { getRoomWidgets } from '../../../data-access/api/widgets-service.js';
  import Widget from '../../../components/Widget.svelte';
  import WaitingComponent from '../../../components/WaitingComponent.svelte';
  
  export let data;

  onMount(async () => {
    loading.set(true);
    var result = await getRoomWidgets(data.id);
    widgets.set(result);
    loading.set(false);
  });
</script>

<div class="w-full flex flex-row flex-wrap items-center justify-center">
  <WaitingComponent>
    {#each $widgets as widget}
      <Widget bind:widget={widget}>
        <div>{widget.id}</div>
      </Widget>
    {/each}
  </WaitingComponent>
</div>