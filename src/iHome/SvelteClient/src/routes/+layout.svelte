<script>
  import { onMount } from "svelte";
  import "../app.postcss";
  import PageNavbar from "../components/PageNavbar.svelte";
  import "./styles.css";
  import auth from "../data-access/auth-service";
  import { DarkMode } from "flowbite-svelte";
  import { isAuthenticated, user } from "../data-access/store";

  onMount(async () => {
    const client = await auth.createClient();

    isAuthenticated.set(await client.isAuthenticated());
    user.set(await client.getUser());
  });
</script>

<DarkMode class="hidden" />

<div class="app">
  <PageNavbar />

  <main>
    <slot />
  </main>

  <footer>
    
  </footer>
</div>