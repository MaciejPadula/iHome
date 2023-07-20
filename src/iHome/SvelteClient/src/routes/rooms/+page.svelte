<script lang="ts">
  import { loading, rooms } from "../../data-access/store";
  import Room from "../../components/Room.svelte";
  import { onMount } from "svelte";
  import { getRooms } from "../../data-access/api/rooms-service";
  import WaitingComponent from "../../components/WaitingComponent.svelte";

  onMount(async () => {
    loading.set(true);
    const result = await getRooms();
    rooms.set(result);
    loading.set(false);
  });

  const roomUrl = (id: string) => `rooms/${id}`;
</script>

<WaitingComponent>
  {#each $rooms as room}
    <Room bind:room={room} urlGenerator={roomUrl} />
  {/each}
</WaitingComponent>