<script lang="ts">
  import { getRoomWidgets } from '../../../data-access/api/widgets-service.js';
  import Widget from '../../../components/Widget.svelte';
  import WaitingComponent from '../../../components/WaitingComponent.svelte';
  import { rooms, widgets } from '../rooms-store';
  import GuardComponent from '../../../components/GuardComponent.svelte';
  import { executeWithLoadingAsync } from '../../../data-access/loading-service';
  import auth from '../../../data-access/auth-service';
  import type { RoomModel } from '../../../models/room';
  
  export let data;
  let room: RoomModel;

  auth.subscribeWhenLogged(() => loadWidgets());

  async function loadWidgets() {
    await executeWithLoadingAsync(async () => {
      room = $rooms.filter(r => r.id == data.id)[0];

      var result = await getRoomWidgets(data.id);
      widgets.set(result);
    });
  }
</script>

<svelte:head>
	<title>iHome - {room.name}</title>
	<meta name="description" content="iHome" />
</svelte:head>

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