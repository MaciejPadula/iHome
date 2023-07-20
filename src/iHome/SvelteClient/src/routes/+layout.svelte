<script>
  import "../app.postcss";
  import "./styles.css";
  import { DarkMode } from 'flowbite-svelte';
</script>

<div class="app">
  <main>
    <DarkMode />
    <slot />
  </main>

  <footer>
    
  </footer>
</div>